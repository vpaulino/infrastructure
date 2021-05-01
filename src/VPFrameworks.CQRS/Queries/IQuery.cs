using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Frameworks.Cqrs.Queries
{
    /// <summary>
    /// Represents some query that needs to be done
    /// </summary>
    public interface IQuery
    {
        /// <summary>
        /// Gets the  number of results that is going to skip until fetching of data happens
        /// </summary>
        int Skip { get; }

        /// <summary>
        /// Gets the number of records to fetch
        /// </summary>
        int Take { get; set; }

        /// <summary>
        /// Gets or sets the filter expression
        /// </summary>
        Expression<Func<object, bool>> Filter { get;  }
    }
}
