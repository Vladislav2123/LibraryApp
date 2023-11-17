namespace LibraryApp.Application.Feauters.Books.Querries.Dto
{
	public class BookContentVm
	{
		public string FileName { get; set; }
		public byte[] Bytes { get; set; }
		public string ContentType => "application/pdf";
	}
}
