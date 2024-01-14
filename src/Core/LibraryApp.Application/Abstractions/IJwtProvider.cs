using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Abstractions;

public interface IJwtProvider
{
	string GetJwtToken(User user);
}
