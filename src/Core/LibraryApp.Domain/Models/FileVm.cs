namespace LibraryApp.Domain.Models
{
	public class FileVm
	{
		public string FileName { get; init; }
		public string ContentType { get; init; }
		public byte[] Bytes { get; init; }
	}
}
