namespace LibraryApp.Application.Feauters.Users.Queries.Login;

public class LoginResponse
{
	public Guid UserId { get; set; }
	public string Token { get; set; }
}
