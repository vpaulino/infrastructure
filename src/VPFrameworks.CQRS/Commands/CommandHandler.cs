using Frameworks.Cqrs;
using System.Threading;
using System.Threading.Tasks;

namespace Frameworks.Cqrs.Commands
{
    /// <summary>
    /// Entity that knows How to handle a command 
    /// </summary>
    /// <typeparam name="TCom"></typeparam>
    public class CommandHandler<TCom> : ICommandHandler where TCom : class, ICommand 
    {
        private IUnitOfWork<TCom> unitOfWork;

        /// <summary>
        /// creates a new instance of a command handler
        /// </summary>
        /// <param name="unitOfWork"></param>
        public CommandHandler(IUnitOfWork<TCom> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        ///  EXecute the logic of the command
        /// </summary>
        /// <param name="command"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public virtual async Task Handle<TCommand>(TCommand command, CancellationToken token) where TCommand : ICommand
        {
            await this.unitOfWork.Execute(command as TCom, token);
        }
 
    }


}