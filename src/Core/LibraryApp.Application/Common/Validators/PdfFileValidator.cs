using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace LibraryApp.Application.Common.Validators
{
	public class PdfFileValidator : AbstractValidator<IFormFile>
	{
        public PdfFileValidator()
        {
            RuleFor(file => file.Length)
                .NotNull();

            RuleFor(file => file.ContentType)
                .NotNull()
                .Equal("application/pdf")
                .WithMessage("Unsopported content type");
        }
    }
}
