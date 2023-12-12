using LibraryApp.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using LibraryApp.API.Authorization.Common;

namespace LibraryApp.API.Authorization.ReviewEdit
{
	public class EditReviewHandler : BaseResourceEditHandler<EditReviewRequirement, Guid>
	{
		private readonly ILibraryDbContext _dbContext;

		public EditReviewHandler(ILibraryDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		protected override async Task<bool> IsResourceAuthor(AuthorizationHandlerContext context, Guid resourceId)
		{
			if (Guid.TryParse(context.User.FindFirstValue(ClaimTypes.Actor), out Guid userId) == false)
				return false;

			var review = await _dbContext.Reviews
				.FindAsync(resourceId);

			if (review == null) return false;

			return review.UserId == userId;
		}
	}
}
