using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastrutureClients.Persistence.Abstractions
{
    /// <summary>
    ///  If some entity was expected to exist but its not there 
    /// </summary>
    public class EntityNotFoundException : EntityException
    {
        /// <summary>
        /// Gets or sets the Id of the entity
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets of sets the Type of the entity
        /// </summary>
        public string Type { get; set; }
        
        /// <summary>
        /// Creates an instance of <see cref="EntityNotFoundException"/>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        public EntityNotFoundException(string id, string type) : this(id, type, "Entity not found")
        {
            
        }

        /// <summary>
        /// Creates an instance of <see cref="EntityNotFoundException"/>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="message"></param>

        public EntityNotFoundException(string id, string type, string message)
            : base(null, message)
        {
            this.Id = id;
            this.Type = type;
        }

        /// <summary>
        /// Creates an instance of <see cref="EntityNotFoundException"/>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public EntityNotFoundException(string id, string type, string message, Exception inner) : base(null, message, inner)
        {
            this.Id = id;
            this.Type = type;
        }

        
    }
}
