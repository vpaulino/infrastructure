using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VPFrameworks.Persistence.Abstractions
{
    /// <summary>
    /// Instance of object that as all the data about a set of objects that represents a page of information
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PaginationCollection<T>
    {
       
        /// <summary>
        /// creates an instance of pagination collection
        /// </summary>
        /// <param name="results"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalPages"></param>
        public PaginationCollection(IEnumerable<T> results, int pageNumber, long pageSize, long totalPages)
        {
            this.Data = results;
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.Total = totalPages;
        }

        /// <summary>
        /// Gets or sets data records
        /// </summary>
        public IEnumerable<T> Data { get; set; }

        /// <summary>
        /// Gets or sets the total ammount of records 
        /// </summary>
        public long Total { get; set; }

        /// <summary>
        /// Gets or sets the Size of the page returned
        /// </summary>
        public long PageSize { get; set; }

        /// <summary>
        /// Gets or sets the page number
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// total number of records on Data
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return this.Data.Count();
        }
    }
}
