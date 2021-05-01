using System.Threading;
using System.Threading.Tasks;

namespace VPFrameworks.Persistence.Abstractions
{
    /// <summary>
    /// Represents a set of write operations to a repository
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public interface IWriteRepository<TId>
    {
        /// <summary>
        /// Creates an instance of object. if that entity already exists if should throw exception 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task CreateAsync<T>(T entity, CancellationToken token);

        /// <summary>
        /// Creates an instance and if already exists then it will be updated
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task CreateOrUpdateAsync<T>(T entity, CancellationToken token);

        /// <summary>
        /// Delete an entity by is Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task DeleteAsync(TId id, CancellationToken token);
    }
}
