namespace uLocate.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class PagingCollection<T> : IEnumerable<T>
    {
        #region fields

        private const int DefaultPageSize = 10;

        private readonly IEnumerable<T> _collection;

        private int _pageSize = DefaultPageSize;

        #endregion

        #region properties

        /// <summary>
        /// Gets or sets page size
        /// </summary>
        public int PageSize
        {
            get
            {
                return this._pageSize;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException();
                }
                this._pageSize = value;
            }
        }

        /// <summary>
        /// Gets pages count
        /// </summary>
        public int PagesCount
        {
            get
            {
                return (int)Math.Ceiling(this._collection.Count() / (decimal)this.PageSize);
            }
        }

        /// <summary>
        /// Gets the total count of all items across all pages
        /// </summary>
        public int TotalCount
        {
            get
            {
                return this._collection.Count();
            }
        }

        #endregion

        #region ctor

        /// <summary>
        /// Creates paging collection and sets page size
        /// </summary>
        public PagingCollection(IEnumerable<T> collection, int pageSize)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            this.PageSize = pageSize;
            this._collection = collection.ToArray();
        }

        /// <summary>
        /// Creates paging collection
        /// </summary>
        public PagingCollection(IEnumerable<T> collection)
            : this(collection, DefaultPageSize)
        {
        }

        #endregion

        #region public methods

        /// <summary>
        /// Returns data by page number
        /// </summary>
        public IEnumerable<T> GetData(int pageNumber)
        {
            if (pageNumber < 0 || pageNumber > this.PagesCount)
            {
                return new T[] { };
            }

            int offset = (pageNumber - 1) * this.PageSize;

            return this._collection.Skip(offset).Take(this.PageSize);
        }

        /// <summary>
        /// Returns number of items on page by number
        /// </summary>
        public int GetCount(int pageNumber)
        {
            return this.GetData(pageNumber).Count();
        }

        #endregion

        #region static methods

        /// <summary>
        /// Returns data by page number and page size
        /// </summary>
        public static IEnumerable<T> GetPaging(IEnumerable<T> collection, int pageNumber, int pageSize)
        {
            return new PagingCollection<T>(collection, pageSize).GetData(pageNumber);
        }

        /// <summary>
        /// Returns data by page number
        /// </summary>
        public static IEnumerable<T> GetPaging(IEnumerable<T> collection, int pageNumber)
        {
            return new PagingCollection<T>(collection, DefaultPageSize).GetData(pageNumber);
        }

        #endregion

        #region IEnumerable<T> Members

        /// <summary>
        /// Returns an enumerator that iterates through collection
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            return this._collection.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through collection
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}