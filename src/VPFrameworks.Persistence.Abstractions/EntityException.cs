using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastrutureClients.Persistence.Abstractions
{
     /// <summary>
     /// Represents the base type of 
     /// </summary>
    public abstract class EntityException  
    {
        /// <summary>
        /// Gets or sets the entity related to this exception
        /// </summary>
        public object Entity { get; set; }

      
        /// <summary>
        /// Creates an instance of Entity Exception
        /// </summary>
        protected EntityException()
        {

        }


        /// <summary>
        /// Creates an instance of Entity Exception
        /// </summary>
        protected EntityException(object entity) 
        {
            this.Entity = entity;
        }

        /// <summary>
        /// Creates an instance of Entity Exception
        /// </summary>
        protected EntityException(object entity, string message) 
            
        {
            this.Entity = entity;
        }

        /// <summary>
        /// Creates an instance of Entity Exception
        /// </summary>
        protected EntityException(object entity, string message, Exception inner)
            
        {
            this.Entity = entity;
        }
    }
}
