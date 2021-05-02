using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastrutureClients.Persistence.Abstractions
{
    /// <summary>
    /// When there is a conflict cause by some entity when accessing in the database this exception should be thrown 
    /// </summary>
    public class EntityConflictException : EntityException
    {
       

       
        /// <summary>
        /// Creates an instance of <see cref="EntityConflictException"/>
        /// </summary>
        /// <param name="entity"></param>
        public EntityConflictException(object entity) : base(entity)
        {
        
        }

        /// <summary>
        /// Creates an instance of <see cref="EntityConflictException"/>
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        public EntityConflictException(object entity, string message)
            : base(entity, message)
        {

        }

        /// <summary>
        /// Creates an instance of <see cref="EntityConflictException"/>
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public EntityConflictException(object entity, string message, Exception inner) : base(entity, message, inner)
        {
        
        }

        //protected EntityConflictException(
        //  System.Runtime.Serialization.SerializationInfo info,
        //  System.Runtime.Serialization.StreamingContext context)
        //    : base(info, context) 
        //{
        
        //}
    }
}
