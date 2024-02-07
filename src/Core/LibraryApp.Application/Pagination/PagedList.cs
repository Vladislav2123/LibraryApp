namespace LibraryApp.Application.Pagination;

public class PagedList<T>
{
    public PagedList(List<T> items, int total, Page page)
    {
        Items = items;
        Total = total;
        Page = page.number;
        Size = page.size;
    }

    public List<T> Items { get; }

    /// <summary>
    /// Total Items amount.
    /// </summary>
    public int Total { get; }

    /// <summary>
    /// Current page number.
    /// </summary>
    public int Page { get; }

    /// <summary>
    /// Size of page.
    /// </summary>
    public int Size { get; }
}
