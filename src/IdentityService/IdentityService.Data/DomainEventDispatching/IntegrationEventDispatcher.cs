using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using IdentityService.Data.DatabaseContext;
using IdentityService.Domain.DomainEntities;
using IdentityService.Domain.DomainEntities.OutboxPattern;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SharedKernel.DomainContracts;

namespace IdentityService.Data.DomainEventDispatching;

public class IntegrationEventDispatcher : IDomainEventsDispatcher
{
    private readonly ApplicationDbContext _context;
    private readonly IMediator _mediator;
    private readonly ILifetimeScope _scope;

    public IntegrationEventDispatcher(IMediator mediator, ILifetimeScope scope, DbContext context)
    {
        _mediator = mediator;
        _scope = scope;
        _context = context as ApplicationDbContext;
    }

    public async Task DispatchEventsAsync()
    {
        var greeterActivitySource = new ActivitySource("OtPrGrJa");
        using var activity = greeterActivitySource.StartActivity("RegisterUserCommandHandler");

        var domainEntities = _context.ChangeTracker
            .Entries<BasicDomainEntity<long>>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any()).ToList();

        var domainEntitiesWithGuid = _context.ChangeTracker
            .Entries<BasicDomainEntity<Guid>>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any()).ToList();

        var domainEvents = domainEntities.SelectMany(x => x.Entity.DomainEvents).ToList();

        var domainEventsWithGuid = domainEntitiesWithGuid.SelectMany(x => x.Entity.DomainEvents).ToList();

        var integrationEvents = new List<IIntegrationEvent<IDomainEvent>>();

        foreach (var intEvent in domainEvents)
        {
            var integrationEventType = typeof(IIntegrationEvent<>);
            var integrationEventWithGenericType =
                integrationEventType.MakeGenericType(intEvent.GetType());
            var integrationEvent = _scope.ResolveOptional(integrationEventWithGenericType, new List<Parameter>
            {
                new NamedParameter("integrationEvent", intEvent)
            });

            if (integrationEvent != null) integrationEvents.Add(integrationEvent as IIntegrationEvent<IDomainEvent>);
        }

        foreach (var guidEvent in domainEventsWithGuid)
        {
            var integrationEventType = typeof(IIntegrationEvent<>);
            var integrationEventWithGenericType =
                integrationEventType.MakeGenericType(guidEvent.GetType());
            var integrationEvent = _scope.ResolveOptional(integrationEventWithGenericType, new List<Parameter>
            {
                new NamedParameter("integrationEvent", guidEvent)
            });

            if (integrationEvent != null)
                integrationEvents.Add(integrationEvent as IIntegrationEvent<IDomainEvent>);
        }

        domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());
        domainEntitiesWithGuid.ForEach(entity => entity.Entity.ClearDomainEvents());

        var tasks = domainEvents.Select(async domainEvent => { await _mediator.Publish(domainEvent); });
        var tasksWithGuid = domainEventsWithGuid.Select(async domainEvent => { await _mediator.Publish(domainEvent); });

        await Task.WhenAll(tasks);
        await Task.WhenAll(tasksWithGuid);

        // 2PC problem is solved by using an outbox table as a queue
        // each and every event that needs to be published so other microservices can react to it
        // the integration event is first inserted into the local Db (the outbox table) in a single transaction
        // then, a background job (Quartz) will read rows from the outbox table and attempt publishing then unto a message queue (or a kafka topic)
        // this is the only way in which we can guarantee delivery (at least once delivery guarantee) without resorting to distributed transactions which are, in most cases, a no go
        // there are frameworks that basically do this for us, but I hate having code that I dont know in my source
        foreach (var integrationEvent in integrationEvents)
        {
            var type = integrationEvent.GetType().FullName;

            if (type == null)
                throw new Exception(
                    "Cannot extract type from the provided assembly, perhaps your integration event cs class is put in a wrong assembly?");

            activity?.SetTag("Integration event handled", integrationEvent.GetType().FullName);

            var data = JsonConvert.SerializeObject(integrationEvent);
            var outboxMessage = new OutboxMessage(
                integrationEvent.IntegrationEvent.OccurredOn,
                type,
                data,
                integrationEvent.IntegrationEvent.TypeOfEvent);

            await _context.OutboxMessages.AddAsync(outboxMessage);

            var ae = new ActivityEvent($"Type of integration event: [ {type} ] added to the outbox");
            activity?.AddEvent(ae);
        }
    }
}