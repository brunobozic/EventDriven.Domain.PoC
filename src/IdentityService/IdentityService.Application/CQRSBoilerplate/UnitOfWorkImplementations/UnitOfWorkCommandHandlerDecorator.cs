using System;
using System.Threading;
using System.Threading.Tasks;
using IdentityService.Data.CustomUnitOfWork.Interfaces;
using IdentityService.Data.DatabaseContext;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel.DomainContracts;

namespace IdentityService.Application.CQRSBoilerplate.UnitOfWorkImplementations;

public class UnitOfWorkCommandHandlerDecorator<T> : ICommandHandler<T> where T : ICommand
{
    private readonly ApplicationDbContext _context;
    private readonly ICommandHandler<T> _decorated;
    private readonly IMyUnitOfWork _unitOfWork;

    public UnitOfWorkCommandHandlerDecorator(
        ICommandHandler<T> decorated,
        IMyUnitOfWork unitOfWork,
        ApplicationDbContext context)
    {
        _decorated = decorated;
        _unitOfWork = unitOfWork;
        _context = context;
    }

    public async Task<Unit> Handle(T command, CancellationToken cancellationToken)
    {
        await _decorated.Handle(command, cancellationToken);

        if (command is InternalCommandBase)
        {
            var internalCommand =
                await _context.InternalCommands.FirstOrDefaultAsync(x => x.Id == command.Id,
                    cancellationToken);

            if (internalCommand != null) internalCommand.ProcessedDate = DateTime.UtcNow;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}