namespace LibraryApp.Tests.Common;
public class TestingHelper
{
	public static string CreateTesingFile(string fileName)
	{
		string tempDirectory = Path.GetTempPath();
		string path = Path.Combine(tempDirectory, fileName);

		File.Create(path).Close();

		return path;
	}
}
