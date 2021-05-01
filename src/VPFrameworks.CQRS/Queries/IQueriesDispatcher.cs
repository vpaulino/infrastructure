using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Frameworks.Cqrs.Queries
{
    /// <summary>
    /// Dispach some Query to the <see cref="IQueryHandler"/>
    /// </summary>
    public interface IQueriesDispatcher
    {
        /// <summary>
        /// Dispache the <see cref="IQuery"/> to the <see cref="IQueryHandler"/> that match the IQuery
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task Dispatch(IQuery query);
    }
}
