using FluentValidation;
using LibraryMVC.ViewModels;

namespace LibraryMVC.FluentValidation
{
    public class BookValidator : AbstractValidator<BookVM>
    {
        public BookValidator() {

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Kitab adin daxil edin.")
                .MaximumLength(150);
            RuleFor(x => x.Author)
            .NotEmpty().WithMessage("Avtorun adin daxil edin.")
            .MaximumLength(100);

            RuleFor(x => x.Genre)
                .NotEmpty().WithMessage("Janri daxil edin.")
                .MaximumLength(50);

            RuleFor(x => x.PublishDate)
                .NotEmpty().WithMessage("Tarix daxil edin")
                .LessThan(DateTime.Now).WithMessage("Tarix sehv yazilib.");

            RuleFor(x => x.PageCount)
                .NotEmpty().WithMessage("Sehife sayin daxil edin")
                .GreaterThan(0).WithMessage("Sehife sayi 1 den cox olmalidi.");




        }
    }
}
