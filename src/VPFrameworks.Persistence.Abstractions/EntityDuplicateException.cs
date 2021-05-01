using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPFrameworks.Persistence.Abstractions
{
    /// <summary>
    /// When creating a new entity on repository and it alread exists
    /// </summary>
    public class EntityDuplicateException : EntityException
    {
       
        /// <summary>
        /// Creates a new instance of <see cref="EntityDuplicateException"/>
        /// </summary>
        /// <param name="entity"></param>
        public EntityDuplicateException(object entity) : base(entity)
        {

        }

        /// <summary>
        /// creates a new instance of <see cref="EntityDuplicateException"/>
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        public EntityDuplicateException(object entity, string message)
            : base(entity, message)
        {

        }


        /// <summary>
        /// creates a new instance of <see cref="EntityDuplicateException"/>
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public EntityDuplicateException(object entity, string message, Exception inner) : base(entity, message, inner)
        {
        
        }

        //protected EntityDuplicateException(
        //  System.Runtime.Serialization.SerializationInfo info,
        //  System.Runtime.Serialization.StreamingContext context)
        //    : base(info, context) 
        //{
        
        //}
    }
}
