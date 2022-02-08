using FluentValidation;
using PlannerApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlannerApp.Shared.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Email is not a valid Email Address");

            RuleFor(p => p.FirstName)
                .NotEmpty()
                .WithMessage("First name is required")
                .MaximumLength(50)
                .WithMessage("First name mustbe less than 50 characters");

            RuleFor(p => p.LastName)
                .NotEmpty()
                .WithMessage("Last name is required")
                .MaximumLength(50)
                .WithMessage("Last name mustbe less than 50 characters");

            RuleFor(p => p.UserName)
                .NotEmpty()
                .WithMessage("User name is required")
                .MaximumLength(150)
                .WithMessage("User name mustbe less than 150 characters");

            RuleFor(p => p.Password)
                .NotEmpty()
                .WithMessage("Password is required")
                .MinimumLength(10)
                .WithMessage("Password must be at least 10 characters long")
                .MaximumLength(100)
                .WithMessage("Password mustbe less than 100 characters");

            RuleFor(p => p.ConfirmPassword)
                .NotEmpty()
                .WithMessage("Confirm Password is required")
                .MinimumLength(10)
                .WithMessage("Confirm Password must be at least 10 characters long")
                .MaximumLength(100)
                .WithMessage("Confirm Password mustbe less than 100 characters")
                .Matches(p => p.Password)
                .WithMessage("Passwords do not match");
        }
    }
}
