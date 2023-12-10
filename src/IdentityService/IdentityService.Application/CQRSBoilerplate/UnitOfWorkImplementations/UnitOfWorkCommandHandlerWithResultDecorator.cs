using System;
using System.Threading;
using System.Threading.Tasks;
using IdentityService.Data.CustomUnitOfWork.Interfaces;
using IdentityService.Data.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using SharedKernel.DomainContracts;

namespace IdentityService.Application.CQRSBoilerplate.UnitOfWorkImplementations;

public class UnitOfWorkCommandHandlerWithResultDecorator<T, TResult> : ICommandHandler<T, TResult>
    where T : ICommand<TResult>
{
    private readonly ApplicationDbContext _context;
    private readonly ICommandHandler<T, TResult> _decorated;

    private readonly IMyUnitOfWork _unitOfWork;

    public UnitOfWorkCommandHandlerWithResultDecorator(
        ICommandHandler<T, TResult> decorated,
        IMyUnitOfWork unitOfWork,
        DbContext context)
    {
        _decorated = decorated;
        _unitOfWork = unitOfWork;
        _context = context as ApplicationDbContext;
    }

    public async Task<TResult> Handle(T command, CancellationToken cancellationToken)
    {
        var result = await _decorated.Handle(command, cancellationToken);

        if (command is InternalCommandBase<TResult>)
        {
            var internalCommand =
                await _context.InternalCommands.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

            if (internalCommand != null) internalCommand.ProcessedDate = DateTime.UtcNow;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return result;
    }
}