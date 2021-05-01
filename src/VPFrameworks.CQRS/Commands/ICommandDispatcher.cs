using System.Threading;
using System.Threading.Tasks;

namespace Frameworks.Cqrs.Commands
{ 
    /// <summary>
    /// Represents the contract of command dispatcher funcionality
    /// </summary>
     public interface ICommandDispatcher
    {
        /// <summary>
        /// Dispatch the command to the correct command handler
        /// </summary>
        /// <param name="command"></param>
        /// <param name="token"></param>
        /// <returns></returns>
         Task Dispatch(ICommand command, CancellationToken token);
    }
}