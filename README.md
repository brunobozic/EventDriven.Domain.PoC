dotnet ef migrations add Initial --project EventDriven.Domain.PoC.Repository.EF
dotnet ef database update --project EventDriven.Domain.PoC.Repository.EF 



## Liveness probe 
- URL: https://localhost:5001/liveness

## Rudimentary HC endpoint
- URL: https://localhost:5001/hc

- URL: https://localhost:5001/healthchecks-ui#/healthchecks

## Jaeger (open tracing)

## Commands

## Domain Events

If the Domain Event should be processed outside of the ongoing transaction, you should define a Notification Object for it. 
This is the object which should be written to the Outbox.

First thing to note is Json.NET library usage to serialize/deserialize event messages. 
Second thing to note are 2 constructors of CustomerRegisteredNotification class. 
First of them is for creating notification based on Domain Event. 
Second of them is to deserialize message from JSON string which is presented in following section regarding processing.

The most important parts to note here are:

All intergration events (events to be consumed by other microservices) must be published to a "Outbox" table.
You can publish [Domain Events] either as part of a single transaction or as part of two different transactions - in which case we are talking about [Integration Events].
If you want to publish it as part of a different transaction, you must create an [IIntegrationEvent<>] object, objects of that type are inserted into the Outbox.
Outbox Item(s) must be handled by a separate background worker/job as part of a separate process - a Quartz background job (Quartz scheduler) is used in this application.
Without a working Quartz scheduler, the outbox items will be piling up and the integration events will not get published. 
See [IntegrationEventDispatcher.cs] for more information.

In a separate proces the Quartz scheduler will make the following actions:

Each Outbox item is read, deserialized, and the object is then produced to a Kafka topic.

1. [Command Handler] defines a single transaction boundary. Transaction is started when Command Handler is invoked and is committed **at the end** of it.
2. Each [Domain Event Handler] is invoked in context of the same transaction boundary.
3. If we want to process something **outside** the original transaction, we need to create a new public integration event ([IIntegrationEvent<>]) based on the original Domain Event

--------------------------------
Items are inserted into the Outbox
– after each Command handling (but BEFORE committing transaction)
– after each Domain Event handling (but WITHOUT committing transaction)
--------------------------------

--------------------------------
TBD: Inbox

Provided you are enqueueing another command as a reaction to a specific domain event this event will get inserted into an "Inbox" database table by Quartz scheduler.
The inserts themselves are handled by [CommandsScheduler.cs].
MediatR handles the commands enqueued and handled in this way by executing code you can find in [CommandsDispatcher.cs].

This is how you would enqueue such a command:

   await _commandsScheduler.EnqueueAsync(new SendAccountVerificationMailCommand(
                Guid.NewGuid(),
                notification.ActivationLink,
                notification.ActivationLinkGenerated,
                notification.UserId,
                notification.Email,
                notification.FirstName,
                notification.LastName
            ));

Inspect [ProcessInternalCommandsCommandHandler.cs] for more detail on how an inbox item is being picked up and handled.

--------------------------------

We need two decorators. The first one will be for command handlers
we also need a second decorator for the Domain Event handler, which will only publish Domain Events at the very end without committing database transaction


## TODO

            // [RegisterApplicationUserCommand] will get handled by [RegisterApplicationUserCommandHandler]
            // that one will fire an event [ApplicationUserCreatedDomainEvent] (when the user is created, the user has not yet confirmed his email so his account is not yet fully activated)
            // this will get handled by [ApplicationUserCreatedDomainEventHandler]
            // this will fire [SendAccountVerificationMailCommand]
            // this will get handled by [SendAccountVerificationMailCommandHandler] that will send an email containing the activation link
            // the flow continues when the user clicks on the provided url (found in an email that we had sent to the user)
            // [EmailVerifiedDomainEvent] is then fired, and is handled by [EmailVerifiedDomainEventHandler] and the user will be marked as "verified"
            // finally a greeting mail is sent to the user
            // this will fire a new command: [SendAWelcomeMailCommand]
            // this command will get handled by [SendAWelcomeMailCommandHandler], that will send a greeting email to the users email address
            // a domain event is fired in response to this => [MarkApplicationUserAsWelcomedDomainEvent] is sent
            // and is handled by [MarkApplicationUserAsWelcomedDomainEventHandler] marking the user as "welcomed"


            // Forgot password Flow


            // Resend activation link Flow

            // User 
            created
            deleted
            address changed
            deactivated, reactivated, activated, undeleted

            // Role
            created
            deleted
            deactivated, reactivated, activated, undeleted
            assigned to user, removed from user

            // Address
            created
            deleted
            deactivated, reactivated, activated, undeleted
            user given address, user changed address

### HiLo on MSSQL
### Guid as PK (generated up front)