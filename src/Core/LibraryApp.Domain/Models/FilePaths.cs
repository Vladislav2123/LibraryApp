namespace LibraryApp.Domain.Models;

public class FilePaths
{
	public const string ConfigSectionKey = "FilePaths";
	public string BooksPath { get; set; }
	public string AvatarsPath { get; set; }
	public string CoversPath { get; set; }
}
