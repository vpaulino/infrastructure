using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfrastrutureClients.Persistence.Abstractions
{
    /// <summary>
    /// This interfaces represents the Domain abstraction to enable concrete repository implementations to read data from a persisted engine
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public interface IReadRepository<TId>
    {
       /// <summary>
       /// Get an Domain entity from a repository implementation of PersistendEntity
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="id"></param>
       /// <param name="token"></param>
       /// <returns></returns>
        Task<T> GetByIdAsync<T>(TId id, CancellationToken token);

        /// <summary>
        ///  Gets a list of Domain entities in a page format ordered by a field and order
        /// </summary>
        /// <typeparam name="T"> Domain type </typeparam>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <param name="orderBy"></param>
        /// <param name="orderDirection"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<PaginationCollection<T>> GetListAsync<T>(int take, int skip, Func<T, object> orderBy, bool orderDirection, CancellationToken token);

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> os Entities
        /// </summary>
        /// <param name="CreatedBiggerThen">lower date</param>
        /// <param name="CreatedlessThen"> upper date</param>
        /// <param name="take">number of records to retrieve</param>
        /// <param name="skip">number of records to skip</param>
        /// <param name="token">cancellation token</param>
        /// <returns></returns>
        Task<PaginationCollection<T>> GetByTimeIntervalAsync<T>(DateTime CreatedBiggerThen, DateTime CreatedlessThen, int take, int skip, CancellationToken token = default);

    }
}
