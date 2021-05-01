using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Frameworks.Cqrs.Commands
{
    interface IContextualizedCommand : ICommand
    {

        /// <summary>
        /// Gets or sets the user that request the creation of this command. Usualy the Api Requests are authorized, if not available should consider anonymus
        /// </summary>
        string CreatedBy { get; }

        /// <summary>
        /// sets the id of the context of this execution. In non exist should consider threadId, if not consider some correlation Id that can be sent from the client. 
        /// </summary>
        string ContextId { get; }

        /// <summary>
        /// Sets the user that request the creation of this command. Usualy the Api Requests are authorized, if not available should consider anonymus
        ///  and 
        /// </summary>
        /// <typeparam name="TOwnerId"></typeparam>
        /// <typeparam name="TContextId"></typeparam>
        /// <param name="ownerId"></param>
        /// <param name="contextId"></param>
        /// <returns></returns>
        Task SetOwnership<TOwnerId, TContextId>(string ownerId, string contextId);


    }
}
