using System.Collections.Generic;

namespace Azure.Storage.Abstractions
{
    /// <summary>
    /// List files operation result
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListResult<T>
    {
        private readonly List<T> payload;

        /// <summary>
        /// creates a new instance of ListResults
        /// </summary>
        public ListResult() : this(new List<T>()) { }

        /// <summary>
        /// creates instance of ListResult
        /// </summary>
        /// <param name="payload"></param>
        public ListResult(IEnumerable<T> payload)
        {
            this.payload = new List<T>(payload);
        }

        /// <summary>
        /// Add a new item to the result
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            this.payload.Add(item);
        }

        /// <summary>
        /// Gets all the items present in the result
        /// </summary>
        public IEnumerable<T> CloudItems { get { return payload; } }
    }
}