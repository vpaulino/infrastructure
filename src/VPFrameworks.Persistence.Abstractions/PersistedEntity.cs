using System;
using System.Collections.Generic;
using System.Text;

namespace VPFrameworks.Persistence.Abstractions
{
    /// <summary>
    /// Represents an instance of an entity that is stored in the repository
    /// </summary>
    
    public class PersistedEntity<TId>
    {
        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        public TId Id { get; set; }

        /// <summary>
        /// Gets or sets the Date when it was created
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the user who created it
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the data when the entity was updated
        /// </summary>
        public DateTime? Updated { get; set; }

        /// <summary>
        /// Gets or sets the user who update the entity
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// Gets or sets the entity version on the repository
        /// </summary>
        public long Version { get; set; }

        /// <summary>
        /// Gets or sets the tags that describe the entity
        /// </summary>
        public string[] Tags { get; set; }

        /// <summary>
        /// Gets or sets the information that the data has changed to be updated
        /// </summary>
        public bool HasChanged { get; set; }
    }
}
