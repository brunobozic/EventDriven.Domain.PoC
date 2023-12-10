using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using IdentityService.Application.ViewModels.ApplicationUsers.Commands;
using IdentityService.Data.DatabaseContext;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OpenTelemetry.Trace;
using SharedKernel.DomainContracts;

namespace IdentityService.Application.CQRSBoilerplate.Command;

public class CommandsDispatcher : ICommandsDispatcher
{
    private readonly ApplicationDbContext _context;
    private readonly IMediator _mediator;

    public CommandsDispatcher(
        IMediator mediator,
        ApplicationDbContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    public async Task DispatchCommandAsync(Guid id)
    {
        var activitySource = new ActivitySource("OtPrGrJa");
        using var activity = activitySource.StartActivity("CommandsDispatcher");

        var internalCommand = await _context.InternalCommands.SingleOrDefaultAsync(x => x.Id == id);
        if (internalCommand == null)
        {
            activity?.AddEvent(new ActivityEvent("Command not found"));
            return;
        }

        try
        {
            var type = Assembly.GetAssembly(typeof(CreateUserCommand)).GetType(internalCommand.Type);
            dynamic command = JsonConvert.DeserializeObject(internalCommand.Data, type);

            internalCommand.ProcessedDate = DateTime.UtcNow;
            activity?.SetTag("Command.Type", internalCommand.Type);
            activity?.SetTag("Command.Id", id.ToString());

            await _mediator.Send(command);
        }
        catch (Exception ex)
        {
            activity?.RecordException(ex);
            activity?.SetStatus(ActivityStatusCode.Error);
            throw;
        }
    }
}