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

		public LoginQueryHandler(ILibraryDbContext dbConext, IJwtProvider jwtProvider)
		{
			_dbConext = dbConext;
			_jwtProvider = jwtProvider;
		}

		public async Task<LoginResponse> Handle(LoginQuery command, CancellationToken cancellationToken)
		{
			var user = await _dbConext.Users
				.FirstOrDefaultAsync(user => user.Email == command.Email, cancellationToken);

			if (user == null) throw new LoginFailedException();

			if (user.Password != command.Password) throw new LoginFailedException();

			LoginResponse response = new LoginResponse()
			{
				UserId = user.Id,
				Token = _jwtProvider.GetJwtToken(user)
			};

			return response;
		}
	}
}
