using System;
using System.Runtime.Serialization;
using VPFrameworks.Persistence.Abstractions;

namespace InfrastrutureClients.Persistence.Repository.MongoDb
{
    /// <summary>
    /// this exception represents the inconsistency state of the data
    /// </summary>
    /// <typeparam name="TOrigin"></typeparam>
    /// <typeparam name="TId"></typeparam>
    [Serializable]
    public class DataInconsistentException<TOrigin, TId> : Exception
    {
        private TOrigin entity;
        private string v;

        /// <summary>
        /// Creates a new instance of DataInconsistentException
        /// </summary>
        public DataInconsistentException()
        {
        }

        /// <summary>
        /// Creates a new instance of DataInconsistentException
        /// </summary>
        public DataInconsistentException(string message) : base(message)
        {
        }


        /// <summary>
        /// Creates a new instance of DataInconsistentException
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="v"></param>
        public DataInconsistentException(TOrigin entity, string v)
        {
            this.entity = entity;
            this.v = v;
        }

        /// <summary>
        /// Creates a new instance of DataInconsistentException
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public DataInconsistentException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Creates a new instance of DataInconsistentException
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected DataInconsistentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}