using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventDriven.Domain.PoC.Application.CQRSBoilerplate.Command.Handlers
{
    public class LoggingCommandHandlerWithResultDecorator<T, TResult> : ICommandHandler<T, TResult>
        where T : ICommand<TResult>
    {
        private readonly ICommandHandler<T, TResult> _decorated;
        private readonly IExecutionContextAccessor _executionContextAccessor;
        private readonly ILogger _logger;

        public LoggingCommandHandlerWithResultDecorator(
            ILogger logger,
            IExecutionContextAccessor executionContextAccessor,
            ICommandHandler<T, TResult> decorated)
        {
            _logger = logger;
            _executionContextAccessor = executionContextAccessor;
            _decorated = decorated;
        }

        public async Task<TResult> Handle(T command, CancellationToken cancellationToken)
        {
            if (command is IRecurringCommand) return await _decorated.Handle(command, cancellationToken);

            using (
                LogContext.Push(
                    new RequestLogEnricher(_executionContextAccessor),
                    new CommandLogEnricher(command)))
            {
                try
                {
                    Log.Warning(
                        "Executing CQRS command {@Command}",
                        command);

                    var result = await _decorated.Handle(command, cancellationToken);

                    Log.Warning("CQRS Command processed successful, result {Result}", result);

                    return result;
                }
                catch (Exception exception)
                {
                    Log.Error(exception, "CQRS Command processing failed, reason: " + exception.Message);
                    throw;
                }
            }
        }

        private class CommandLogEnricher : ILogEventEnricher
        {
            private readonly ICommand<TResult> _command;

            public CommandLogEnricher(ICommand<TResult> command)
            {
                _command = command;
            }

            public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
            {
                logEvent.AddOrUpdateProperty(new LogEventProperty("Command_Id",
                    new ScalarValue($"Command:{_command.Id}")));
                logEvent.AddOrUpdateProperty(new LogEventProperty("Command_Type",
                    new ScalarValue($"Command type:{_command.GetType()}")));
            }
        }

        private class RequestLogEnricher : ILogEventEnricher
        {
            private readonly IExecutionContextAccessor _executionContextAccessor;

            public RequestLogEnricher(IExecutionContextAccessor executionContextAccessor)
            {
                _executionContextAccessor = executionContextAccessor;
            }

            public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
            {
                if (_executionContextAccessor.IsAvailable)
                    logEvent.AddOrUpdateProperty(new LogEventProperty("CorrelationId",
                        new ScalarValue(_executionContextAccessor.CorrelationId)));
            }
        }
    }
}