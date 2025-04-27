using FluentValidation;
using LibraryMVC.ViewModels;

namespace LibraryMVC.FluentValidation
{
    public class BorrowValidator : AbstractValidator<BorrowVM>
    {
        public BorrowValidator()
        {
            RuleFor(x => x.BookId)
                .GreaterThan(0).WithMessage("Kitabin ID sin daxil edin.");

            RuleFor(x => x.ReaderId)
                .GreaterThan(0).WithMessage("Oxuyucunun ID sin daxil edin.");

        

            RuleFor(x => x.ReturnDueDate)
                .GreaterThan(DateTime.Today).WithMessage("Qaytarma tarixi kecmishde ola bilmez.");

            RuleFor(x => x.ReturnDueDate)
                .Must((borrow, returnedDate) =>
                    returnedDate == null || returnedDate >= borrow.BorrowDate
                ).WithMessage("Qaytarma tarixi icare tarixinnen evvel ola bilmez.");
        }
    }
}
