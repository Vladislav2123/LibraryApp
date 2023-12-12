using LibraryApp.API.Authorization.Common;

namespace LibraryApp.API.Authorization.UserEdit
{
	public class EditUserRequirement : BaseResourceEditRequirement
	{
		public EditUserRequirement(bool allowAdmins) 
			: base(allowAdmins) { }
	}
}
