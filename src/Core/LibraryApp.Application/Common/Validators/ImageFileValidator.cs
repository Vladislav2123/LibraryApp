using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace LibraryApp.Application.Common.Validators
{
	public class ImageFileValidator : AbstractValidator<IFormFile>
	{
        public ImageFileValidator()
        {
            RuleFor(file => file.Length)
                .NotNull();

            RuleFor(file => file.ContentType)
                .Must(value => value == "image/jpeg" || value == "image/png")
                .WithMessage("File format must be .jpeg or .png");  
        }
    }
}
