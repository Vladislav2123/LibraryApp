using LibraryApp.Application.Abstractions.Caching;

namespace LibraryApp.DAL.Caching;

public class CacheKeys : ICacheKeys
{
    public const string ConfigSectionKey = "CacheKeys";
    public string? User { get; set;}
    public string? Author { get; set;}
    public string? Book { get; set;}
}
