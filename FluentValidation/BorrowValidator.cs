using FluentValidation;
using LibraryMVC.Models;

namespace LibraryMVC.FluentValidation
{
    public class BorrowValidator : AbstractValidator<Borrow>
    {
        public BorrowValidator()
        {
            RuleFor(x => x.BookId)
                .GreaterThan(0).WithMessage("Kitabin ID sin daxil edin.");

            RuleFor(x => x.ReaderId)
                .GreaterThan(0).WithMessage("Oxuyucunun ID sin daxil edin.");

            RuleFor(x => x.BorrowedDate)
                .LessThanOrEqualTo(DateTime.Today).WithMessage("Icare tarixi gelecekde ola bilmez.");

            RuleFor(x => x.ReturnDueDate)
                .GreaterThan(DateTime.Today).WithMessage("Qaytarma tarixi kecmishde ola bilmez.");

            RuleFor(x => x.ReturnedDate)
                .Must((borrow, returnedDate) =>
                    returnedDate == null || returnedDate >= borrow.BorrowedDate
                ).WithMessage("Qaytarma tarixi icare tarixinnen evvel ola bilmez.");
        }
    }
}
