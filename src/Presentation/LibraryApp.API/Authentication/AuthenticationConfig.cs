namespace LibraryApp.API.Authentication;

public class AuthenticationConfig
{
	public const string ConfigSectionKey = "Authentication";

	public string Issuer { get; init; }
	public string Audience { get; init; }
	public string Key { get; init; }
	public int Validity { get; init; }
}
