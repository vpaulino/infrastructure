using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Frameworks.Cqrs.Queries
{
    /// <summary>
    /// Queries Data 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TId"></typeparam>
    interface IQueries<T, TId>
    {
        /// <summary>
        /// Does pagination over the list operation
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> List(int skip = 0, int take = 10);

        /// <summary>
        /// Filter list operation
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> List(Expression<Func<T, bool>> filter, int skip = 0, int take = 10);

        /// <summary>
        /// Gets an entity by is Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<T> GetById(TId id, CancellationToken token);
    }
}
