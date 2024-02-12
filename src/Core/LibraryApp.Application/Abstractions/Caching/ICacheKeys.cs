namespace LibraryApp.Application.Abstractions.Caching;

public interface ICacheKeys
{
    public string User { get; }
    public string Author { get; }
    public string Book { get; }
}
