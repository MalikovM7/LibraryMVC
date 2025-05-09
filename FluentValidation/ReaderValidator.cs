﻿using FluentValidation;
using LibraryMVC.ViewModels;

namespace LibraryMVC.FluentValidation
{
    public class ReaderValidator :AbstractValidator<ReaderVM>
    {
        public ReaderValidator() {

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Adivizi daxil edin")
                .MaximumLength(50);
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Soyadivizi daxil edin")
                .MaximumLength(50);

            RuleFor(x => x.BirthDate)
                .LessThan(DateTime.Now).WithMessage("Dogum tarixi kecmishde olmalidi");

            RuleFor(x => x.RegistrationDate)
                .LessThanOrEqualTo(DateTime.Today).WithMessage("Registraciya tarixi gelecekde olmamalidi");



        }



    }
}
