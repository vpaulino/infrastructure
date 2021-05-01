using System.Threading;
using System.Threading.Tasks;

namespace Frameworks.Cqrs.Commands
{
    /// <summary>
    /// Definition of  the contract to handle commands
    /// </summary>
    public interface ICommandHandler
    {
        /// <summary>
        /// Operation to handle the command
        /// </summary>
        /// <param name="request"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task Handle<TCommand>(TCommand request, CancellationToken token) where TCommand : ICommand;
    }
}