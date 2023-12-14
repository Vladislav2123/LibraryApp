using LibraryApp.Domain.Enteties;

namespace LibraryApp.Application.Abstractions;

public interface IJwtProvider
{
	string GetJwtToken(User user);
}
