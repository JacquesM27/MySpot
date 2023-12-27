using Humanizer;
using Microsoft.Extensions.Logging;
using MySpot.Application.Abstractions;
using System.Diagnostics;

namespace MySpot.Infrastructure.Logging.Decorators
{
    internal sealed class LoggingCommandHandlerDecorator<TCommand>(ICommandHandler<TCommand> commandHandler,
                                                                   ILogger<ICommandHandler<TCommand>> logger)
        : ICommandHandler<TCommand> where TCommand : class, ICommand
    {
        public async Task HandleAsync(TCommand command)
        {
            var commandName = typeof(TCommand).Name.Underscore();
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            logger.LogInformation("Started handling a command: {CommandName}...", commandName);
            await commandHandler.HandleAsync(command);
            stopwatch.Stop();
            logger.LogInformation("Completed handling a command: {CommandName} in {Elapsed}.", commandName, stopwatch.Elapsed);
        }
    }
}
