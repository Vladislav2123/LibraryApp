namespace LibraryApp.Application.Common.Helpers
{
	public class PagedList<T>
	{
		private const int DEFAULT_PAGE = 1;
		private const int DEFAULT_LIMIT = 10;
		private const int MIN_LIMIT = 5;
		private const int MAX_LIMIT = 20;

		public PagedList(List<T> items, int page, int limit, int totalCount)
		{
			Items = items;
			Page = page;
			Limit = limit;
			TotalCount = totalCount;
		}

		public List<T> Items { get; }
		public int Page { get; }
		public int Limit { get; }
		public int TotalCount { get; }

		public static PagedList<T> Create(List<T> items, int page, int limit)
		{
			var totalCount = items.Count;

			if (page == 0) page = DEFAULT_PAGE;

			if (limit == 0) limit = DEFAULT_LIMIT;
			else limit = Math.Clamp(limit, MIN_LIMIT, MAX_LIMIT);

			var pageItems = items.Skip((page - 1) * limit).Take(limit).ToList();

			return new(pageItems, page, limit, totalCount);
		}
	}
}
