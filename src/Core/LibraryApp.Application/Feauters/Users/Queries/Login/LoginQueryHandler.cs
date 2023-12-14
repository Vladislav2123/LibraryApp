using LibraryApp.Application.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Abstractions;

namespace LibraryApp.Application.Feauters.Users.Queries.Login
{
	public class LoginQueryHandler : IRequestHandler<LoginQuery, LoginResponse>
	{
		private readonly ILibraryDbContext _dbConext;
		private readonly IJwtProvider _jwtProvider;
		private readonly IPasswordProvider _passwordProvider;

		public LoginQueryHandler(
			ILibraryDbContext dbConext, 
			IJwtProvider jwtProvider,
			IPasswordProvider passwordHasher)
		{
			_dbConext = dbConext;
			_jwtProvider = jwtProvider;
			_passwordProvider = passwordHasher;
		}

		public async Task<LoginResponse> Handle(LoginQuery command, CancellationToken cancellationToken)
		{
			var user = await _dbConext.Users
				.FirstOrDefaultAsync(user => user.Email == command.Email, cancellationToken);

			if (user == null) throw new LoginFailedException();

			string passwordHash = _passwordProvider.HashPassword(command.Password, user.PasswordSalt);
			if (passwordHash != user.PasswordHash) throw new LoginFailedException();

			LoginResponse response = new LoginResponse()
			{
				UserId = user.Id,
				Token = _jwtProvider.GetJwtToken(user)
			};

			return response;
		}
	}
}
