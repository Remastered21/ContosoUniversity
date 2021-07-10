using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize) // Constructor
                                                                                    // Cannot create the object PaginatedList<T>
                                                                                    // Cannot run asyncrounous code.
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        // Is this initializing?
        public static async Task<PaginatedList<T>> CreateAsync( // used to create the PaginatedList<T>
            IQueryable<T> source, int pageIndex, int pageSize) // source == items + count parameters as IQueryable
        {
            var count = await source.CountAsync();
            var items = await source.Skip(
                (pageIndex - 1) * pageSize)
                .Take(pageSize).ToListAsync(); // returns only the list containing the page
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}