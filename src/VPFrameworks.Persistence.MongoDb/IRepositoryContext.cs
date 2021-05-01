using MongoDB.Driver;

namespace VPFrameworks.Persistence.MongoDb
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepositoryContext<T>
    {
        /// <summary>
        /// 
        /// </summary>
        string DatabaseName { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        IMongoCollection<T> GetCollection(string name, MongoCollectionSettings settings = null);
    }
}