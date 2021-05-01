using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Frameworks.Cqrs.Commands;

namespace Frameworks.Cqrs
{
    /// <summary>
    /// Represents a unit of work of a specific domain bussiness entity
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IUnitOfWork<in TEntity>
    {
        /// <summary>
        /// Execute the unit of work 
        /// </summary>
        /// <param name="entity">bussiness unit to operate into</param>
        /// <param name="token">cancellation token of the operation</param>
        /// <returns></returns>
        Task Execute(TEntity entity, CancellationToken token);
    }
}
