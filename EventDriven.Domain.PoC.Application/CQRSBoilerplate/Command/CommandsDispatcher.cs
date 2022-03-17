using EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Commands;
using EventDriven.Domain.PoC.Repository.EF.DatabaseContext;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace EventDriven.Domain.PoC.Application.CQRSBoilerplate.Command
{
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
            var internalCommand = await _context.InternalCommands.SingleOrDefaultAsync(x => x.Id == id);

            var type = Assembly.GetAssembly(typeof(CreateUserCommand)).GetType(internalCommand.Type);
            dynamic command = JsonConvert.DeserializeObject(internalCommand.Data, type);

            internalCommand.ProcessedDate = DateTime.UtcNow;

            await _mediator.Send(command);
        }
    }
}