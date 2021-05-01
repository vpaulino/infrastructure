using Frameworks.Cqrs;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace Frameworks.Cqrs.Commands
{
    /// <summary>
    /// Concrete implementation that map the concrete type of <see cref="ICommand"/> to the Handler
    /// </summary>
    public class CommandDispatcher : ICommandDispatcher
    {
        private IDictionary<Type, ICommandHandler> commandsHandlers = new Dictionary<Type, ICommandHandler>();

        /// <summary>
        /// Dispatch the <see cref="ICommand"/> to the ICommandHandler
        /// </summary>
        /// <param name="request">command to be executed</param>
        /// <param name="token">cancellation token</param>
        /// <returns></returns>
        public virtual async Task Dispatch(ICommand request, CancellationToken token)
        {

            if (!this.commandsHandlers.TryGetValue(request.GetType(), out var commandHandler))
            {
                throw new UnknownCommandException(request);
            }

            await commandHandler.Handle(request, token);
        }

        /// <summary>
        /// Enable registration of command handlers to the specific command handler
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="commandHandler"></param>
        public virtual void Register<TCommand>(ICommandHandler commandHandler) where TCommand : ICommand
        {
            this.commandsHandlers.Add(typeof(TCommand), commandHandler);
        }
    }
}