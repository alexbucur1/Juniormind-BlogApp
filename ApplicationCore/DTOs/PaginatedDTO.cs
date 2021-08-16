
using System.Collections.Generic;


namespace BlogApp.Dotnet.ApplicationCore.DTOs
{
    public class PaginatedDTO<T>
    {
        public PaginatedDTO(IEnumerable<T> items, int pageIndex, bool hasNextPage, bool hasPreviousPage, int pageSize = 5)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            HasNextPage = hasNextPage;
            HasPreviousPage = hasPreviousPage;
            Items = items;
        }

        public PaginatedDTO() { }

        public IEnumerable<T> Items { get; set; }

        public int PageIndex { get; private set; }

        public bool HasPreviousPage { get; set; }

        public bool HasNextPage { get; set; }

        public int PageSize { get; private set; }
    }
}
