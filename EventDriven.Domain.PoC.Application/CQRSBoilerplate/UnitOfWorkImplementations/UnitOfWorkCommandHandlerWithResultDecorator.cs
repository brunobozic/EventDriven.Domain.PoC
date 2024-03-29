﻿using System;
using System.Threading;
using System.Threading.Tasks;
using EventDriven.Domain.PoC.Repository.EF.CustomUnitOfWork;
using EventDriven.Domain.PoC.Repository.EF.DatabaseContext;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using Microsoft.EntityFrameworkCore;

namespace EventDriven.Domain.PoC.Application.CQRSBoilerplate.UnitOfWorkImplementations
{
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
}