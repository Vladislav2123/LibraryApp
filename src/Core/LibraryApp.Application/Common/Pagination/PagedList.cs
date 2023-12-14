namespace LibraryApp.Application.Common.Pagination;

    public class PagedList<T>
    {
        private const int DefaultPage = 1;
        private const int DefaultSize = 10;
        private const int MinSize = 5;
        private const int MaxSize = 20;

        public PagedList(List<T> items, int total, Page page)
        {
            Items = items;
            Total = total;
            Page = page.number;
            Size = page.size;
        }

        public List<T> Items { get; }
        public int Total { get; }
        public int Page { get; }
        public int Size { get; }

        public static PagedList<T> Create(List<T> items, Page page)
        {
            var totalCount = items.Count;

            if (page.number == 0) page.number = DefaultPage;

            if (page.size == 0) page.size = DefaultSize;
            else page.size = Math.Clamp(page.size, MinSize, MaxSize);

            var pageItems = items.Skip((page.number - 1) * page.size).Take(page.size).ToList();

            return new(pageItems, totalCount, page);
        }
    }
