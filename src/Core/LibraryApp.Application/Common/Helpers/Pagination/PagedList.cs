namespace LibraryApp.Application.Common.Helpers.Pagination
{
    public class PagedList<T>
    {
        private const int DEFAULT_PAGE = 1;
        private const int DEFAULT_SIZE = 10;
        private const int MIN_SIZE = 5;
        private const int MAX_SIZE = 20;

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

			if (page.number == 0) page.number = DEFAULT_PAGE;

			if (page.size == 0) page.size = DEFAULT_SIZE;
			else page.size = Math.Clamp(page.size, MIN_SIZE, MAX_SIZE);

			var pageItems = items.Skip((page.number - 1) * page.size).Take(page.size).ToList();

			return new(pageItems, totalCount, page);
		}
	}
}
