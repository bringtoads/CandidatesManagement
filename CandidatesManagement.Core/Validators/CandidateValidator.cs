using CandidatesManagement.Core.Models;
using FluentValidation;

namespace CandidatesManagement.Core.Validators
{
    public class CandidateValidator : AbstractValidator<Candidate>
    {
        public CandidateValidator()
        {
            RuleFor(c => c.FirstName).NotEmpty();
            RuleFor(c => c.LastName).NotEmpty();
            RuleFor(c => c.Email).NotEmpty().EmailAddress();
            RuleFor(c => c.PhoneNumber).NotEmpty();
            RuleFor(c => c.Comment).NotEmpty();
        }
    }
}
