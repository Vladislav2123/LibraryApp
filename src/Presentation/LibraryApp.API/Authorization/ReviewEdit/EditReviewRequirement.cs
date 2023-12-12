using LibraryApp.API.Authorization.Common;

namespace LibraryApp.API.Authorization.ReviewEdit
{
	public class EditReviewRequirement : BaseResourceEditRequirement
	{
		public EditReviewRequirement(bool allowAdmins) 
			: base(allowAdmins) { }
	}
}
