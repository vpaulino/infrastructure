using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Frameworks.Cqrs.Queries
{
    /// <summary>
    /// Knows how to handle a specific query
    /// </summary>
    public interface IQueryHandler
    {
        /// <summary>
        /// Execute the handling of the query
        /// </summary>
        /// <param name="request"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task Handle(IQuery request, CancellationToken token);
    }
}
