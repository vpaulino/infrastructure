using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using VPFrameworks.Persistence.Abstractions;

namespace InfrastrutureClients.Persistence.Repository.MongoDb
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class MongoDbRepository<TId, TEntity> : IReadRepository<TId>, IWriteRepository<TId> where TEntity : PersistedEntity<TId>
    {
        IMongoCollection<TEntity> collection;
        FilterDefinitionBuilder<TEntity> filterBuilder = new FilterDefinitionBuilder<TEntity>();
        DatabaseSettings options;
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="options"></param>
        public MongoDbRepository(DatabaseSettings options)
        {
            this.options = options;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void SetupConnection()
        {
            this.collection = new MongoClient(options.ConnectionString).GetDatabase(options.DataBaseName).GetCollection<TEntity>(options.SetName);
                
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Setup()
        {
            MapModel();
            SetupConnection();
            TryCreateIndexes();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void MapModel()
        {
            var pack = new ConventionPack();
            pack.Add(new IgnoreExtraElementsConvention(true));
            ConventionRegistry.Register($"{typeof(TEntity).Name}", pack, t => true);

            if (BsonClassMap.IsClassMapRegistered(typeof(PersistedEntity<TId>)))
                return;

            BsonClassMap.RegisterClassMap<PersistedEntity<TId>>(cm =>
            {
                cm.AutoMap();
                cm.SetIsRootClass(true);

                MapId(cm);
                cm.SetIgnoreExtraElements(true);
                cm.MapProperty(x => x.Created)
                    .SetSerializer(new DateTimeSerializer(DateTimeKind.Utc, BsonType.String));
                cm.MapProperty(x => x.Updated)
                    .SetSerializer(new NullableSerializer<DateTime>(new DateTimeSerializer(DateTimeKind.Utc, BsonType.String)));
                cm.UnmapMember(m => m.HasChanged);

                var entityType = typeof(PersistedEntity<TId>);
                entityType.Assembly.GetTypes()
                    .Where(type => entityType.IsAssignableFrom(type)).ToList()
                    .ForEach(type => cm.AddKnownType(type));

                RegisterRepositoryClassMap(cm);
            });
        }

        /// <summary>
        /// Maps the id member serialization and generation mode
        /// </summary>
        /// <param name="cm"></param>
        protected virtual void MapId(BsonClassMap<PersistedEntity<TId>> cm)
        {
            cm.MapIdMember(p => p.Id)
                                .SetIdGenerator(StringObjectIdGenerator.Instance)
                                .SetSerializer(new StringSerializer(BsonType.String));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cm"></param>
        protected abstract void RegisterRepositoryClassMap(BsonClassMap<PersistedEntity<TId>> cm);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="indexModel"></param>
        /// <returns></returns>
        protected virtual string TryCreateIndex(string name, CreateIndexModel<TEntity> indexModel) {
            
            var cursor = this.collection.Indexes.List();
            var indexes = cursor.ToList();

            if (!indexes.Any((doc) => doc.Contains(name)))
            {
                return this.collection.Indexes.CreateOne(indexModel);
            }

            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual string TryCreateIndexes()
        {
            var cursor = this.collection.Indexes.List();
            var indexes = cursor.ToList();
            string result = string.Empty;

            

            if (!indexes.Any((doc) => doc.Contains("Created_1")))
            {
                IndexKeysDefinitionBuilder<TEntity> builder = new IndexKeysDefinitionBuilder<TEntity>();

                var index = builder
                      .Ascending((entity) => entity.Created);
                result = string.Concat(result, this.collection.Indexes.CreateOne(new CreateIndexModel<TEntity>(index, new CreateIndexOptions() { Background = true })));
            }

            if (!indexes.Any((doc) => doc.Contains("CreatedBy_1")))
            {
                IndexKeysDefinitionBuilder<TEntity> builder = new IndexKeysDefinitionBuilder<TEntity>();

                var index = builder
                      .Ascending((entity) => entity.CreatedBy);
                result = string.Concat(result, this.collection.Indexes.CreateOne(new CreateIndexModel<TEntity>(index, new CreateIndexOptions() { Background = true })));
            }

            return result;
        }


        /// <summary>
        /// Maps a persistedEntity to a domain entity 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected abstract T MapToDomainEntity<T>(TEntity entity);

        /// <summary>
        /// Maps a persistedEntity to a domain entity 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected abstract IEnumerable<T> MapToDomainEntities<T>(IEnumerable<TEntity> entity);


        /// <summary>
        /// Maps a domain T entity to TEntity database type.
        /// </summary>
        /// <remarks>
        /// This 
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected abstract TEntity MapToPersistedEntity<T>(T entity);

        /// <summary>
        /// Maps domain to persisted entities
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        protected abstract IEnumerable<TEntity> MapToPersistedEntities<T>(IEnumerable<T> entities);

        /// <summary>
        /// Adds a new object to the collection
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task CreateAsync<T>(T entity, CancellationToken token)
        {
            TEntity dbEntity = MapToPersistedEntity<T>(entity);
            dbEntity.Created = DateTime.UtcNow;
            await this.collection.InsertOneAsync(dbEntity, new InsertOneOptions() { BypassDocumentValidation = true }, token);
        }

        /// <summary>
        /// Adds a new object to the collection
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task CreateBatchAsync(IEnumerable<TEntity> entities, CancellationToken token)
        {
            await this.collection.InsertManyAsync(entities, new InsertManyOptions() { BypassDocumentValidation = false, IsOrdered = false }, token);
        }

        /// <summary>
        /// Deletes the entity
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task DeleteAsync(TId id, CancellationToken token)
        {

            FilterDefinition<TEntity> definition = filterBuilder.Eq<TId>((entity) => entity.Id, id);

            await this.collection.DeleteOneAsync(definition, token);
        }

        /// <summary>
        /// Gets the entity
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<T> GetByIdAsync<T>(TId id, CancellationToken token)
        {
            FilterDefinition<TEntity> definition = filterBuilder.Eq<TId>((entity) => entity.Id, id);
            var cursor = await this.collection.FindAsync<TEntity>(definition);

            return MapToDomainEntity<T>(await cursor.FirstOrDefaultAsync(token));
        }

        

        /// <summary>
        /// Get entities that were creates in this interval of time
        /// </summary>
        /// <param name="CreatedBiggerThen"></param>
        /// <param name="CreatedlessThen"></param>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<PaginationCollection<T>> GetByTimeIntervalAsync<T>(DateTime CreatedBiggerThen, DateTime CreatedlessThen, int take, int skip, CancellationToken token = default)
        {
            var datesInterval = filterBuilder.And(filterBuilder.Gte<DateTime>((entity) => entity.Created, CreatedBiggerThen), filterBuilder.Lte<DateTime>((entity) => entity.Created, CreatedlessThen));
            var results = await this.GetByFilterDefinition(datesInterval, Builders<TEntity>.Sort.Ascending((ent) => ent.Created), skip, take, token);
            var domainResults = new PaginationCollection<T>(MapToDomainEntities<T>(results.Data), results.PageNumber, results.PageSize, results.Total);
            return domainResults;
        }

        /// <summary>
        /// List entities by page
        /// </summary>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <param name="orderBy"></param>
        /// <param name="orderDirection"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<PaginationCollection<T>> GetListAsync<T>(int take, int skip, Func<T, object> orderBy, bool orderDirection, CancellationToken token)
        {
            var results = await this.GetByFilterDefinition(Builders<TEntity>.Filter.Empty, Builders<TEntity>.Sort.Ascending((ent) => orderBy(MapToDomainEntity<T>(ent))), skip, take, token);
            var domainResults = new PaginationCollection<T>(MapToDomainEntities<T>(results.Data), results.PageNumber, results.PageSize, results.Total);
            return domainResults;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterDefinition"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        protected virtual async Task<IEnumerable<TEntity>> GetByFilterDefinition(FilterDefinition<TEntity> filterDefinition, CancellationToken token) {

            var results = await this.collection.FindAsync<TEntity>(filterDefinition, null, token);

            return await results.ToListAsync<TEntity>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterDefinition"></param>
        /// <param name="orderBy"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        protected virtual async Task<PaginationCollection<TEntity>> GetByFilterDefinition(FilterDefinition<TEntity> filterDefinition, SortDefinition<TEntity> orderBy, int? skip = null, int? take = null, CancellationToken token = default) 
        {

            if (orderBy == null)
                orderBy = Builders<TEntity>.Sort.Ascending((ent) => ent.Created);

            var countFacet = AggregateFacet.Create("count",
              PipelineDefinition<TEntity, AggregateCountResult>.Create(new[]
              {
                PipelineStageDefinitionBuilder.Count<TEntity>()
              }));


          
            AggregateFacet<TEntity, TEntity> dataFacet = null;
            dataFacet = CreateFacetDataWithPagination(skip, take, orderBy);

            if (dataFacet == null)
            {
                dataFacet = AggregateFacet.Create("data",
                PipelineDefinition<TEntity, TEntity>.Create(new[]
                {
                  PipelineStageDefinitionBuilder.Sort(orderBy),
                
                }));
            }

            var aggregation = await collection.Aggregate()
                .Match(filterDefinition)
                .Facet(countFacet, dataFacet)
                .ToListAsync();

            var aggregationResult = aggregation.FirstOrDefault();

            long? count = 0;
            IReadOnlyList<TEntity> data = new List<TEntity>();
            if (aggregation.Any())
            {
                count = aggregationResult
               .Facets.First(x => x.Name == "count")
               .Output<AggregateCountResult>()
               .FirstOrDefault()?
               .Count;

                data = aggregationResult
              .Facets.FirstOrDefault(x => x.Name == "data")?
              .Output<TEntity>();
            }

            var totalPages = (int)Math.Ceiling(!count.HasValue ? 0 : (double)count / (take.HasValue ? take.Value : 1));
           
            return new PaginationCollection<TEntity>(new List<TEntity>(data), skip  != null && skip.HasValue ?  skip.Value : 0, count.HasValue ? count.Value : 0, totalPages);
            
        }

        private AggregateFacet<TEntity, TEntity> CreateFacetDataWithPagination(int? skip, int? take, SortDefinition<TEntity> orderBy)
        {
            if (!skip.HasValue && !take.HasValue)
                return null;

           return  AggregateFacet.Create("data",
                PipelineDefinition<TEntity, TEntity>.Create(new[]
                {
                 // PipelineStageDefinitionBuilder.Sort(orderBy),
                  PipelineStageDefinitionBuilder.Skip<TEntity>(skip.Value > 0 ? (skip.Value - 1) * take.Value : 0),
                  PipelineStageDefinitionBuilder.Limit<TEntity>(take.Value)
                }));
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        protected virtual async Task<bool> IsValidToUpdate(TEntity entity, CancellationToken token)
        {
            if (!entity.HasChanged)
                return false;

            var id = Builders<TEntity>.Filter.Eq((evt) => evt.Id, entity.Id);
            var version = Builders<TEntity>.Filter.Gte((evt) => evt.Version, entity.Version);
            Builders<TEntity>.Filter.And(id, version);
            var results = await this.GetByFilterDefinition(Builders<TEntity>.Filter.And(id, version), Builders<TEntity>.Sort.Ascending((evt) => evt.Created), 0, 1, token);

            return results.Count() == 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"> Domain type</typeparam>
        /// <param name="entity"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public virtual async Task CreateOrUpdateAsync<T>(T entity, CancellationToken token)
        {
            TEntity entityToUpdate = MapToPersistedEntity<T>(entity);
            if (!await IsValidToUpdate(entityToUpdate, token))
                throw new DataInconsistentException<T, TId>(entity, "There was already other update executed");

            UpdateDefinition<TEntity> updateDefinition = CreateEntityUpdateDefinition(entityToUpdate);

            await this.UpdateByAsync((ent) => ent.Id.Equals(entityToUpdate.Id) && ent.Version < entityToUpdate.Version, updateDefinition, token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityToUpdated"></param>
        /// <returns></returns>
        protected virtual UpdateDefinition<TEntity> CreateEntityUpdateDefinition(TEntity entityToUpdated) {

            var updateDefinition = Builders<TEntity>.Update.Combine(Builders<TEntity>.Update.Set((evt) => evt.Updated, entityToUpdated.Updated));
            updateDefinition = Builders<TEntity>.Update.Combine(updateDefinition, Builders<TEntity>.Update.Set((evt) => evt.UpdatedBy, entityToUpdated.UpdatedBy));
            updateDefinition = Builders<TEntity>.Update.Combine(updateDefinition, Builders<TEntity>.Update.Set((evt) => evt.Version, entityToUpdated.Version));

            return updateDefinition;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="updateDefinition"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        protected virtual async Task<long> UpdateByAsync(Expression<Func<TEntity, bool>> filter, UpdateDefinition<TEntity> updateDefinition, CancellationToken token)
        {
            var updateResult = await this.collection.UpdateManyAsync<TEntity>(filter, updateDefinition, new UpdateOptions() { IsUpsert = false, BypassDocumentValidation = false,   });

            if (updateResult.IsModifiedCountAvailable)
            {
                return updateResult.ModifiedCount;
            }
            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(string entityId, CancellationToken ct) {
            var exists = await this.collection.FindAsync<TEntity>((ent) => ent.Id.Equals(entityId), new FindOptions<TEntity, TEntity>() { Limit = 1, }, ct);
            return await exists.AnyAsync<TEntity>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderBy"></param>
        /// <param name="orderDirection"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<PaginationCollection<TEntity>> GetByCreator(string userId, Func<TEntity, object> orderBy, int orderDirection, int skip, int take, CancellationToken token)
        {
            var userFilter  = Builders<TEntity>.Filter.Eq<string>((entity) => entity.CreatedBy, userId);
            var sortDef = orderDirection == 0 ? Builders<TEntity>.Sort.Ascending((ent)=> orderBy(ent)) : Builders<TEntity>.Sort.Descending((ent) => orderBy(ent));
            return await this.GetByFilterDefinition(userFilter, sortDef, skip, take, token);
        }
    }
}
