using FluentValidation;
using Fundo.Applications.Domain.Models;
using Fundo.Applications.Repository.Entity;
using Fundo.Applications.Repository.Interface;

namespace Fundo.Applications.Domain.Validations;

public class LoanValidation : AbstractValidator<RequestLoan>
{ 
    private readonly IApplicantRepository _applicantRepository;
    public LoanValidation(IApplicantRepository applicantRepository)
    {
        _applicantRepository = applicantRepository;

        RuleFor(x => x).Cascade(CascadeMode.Continue)
         .Must(x => !string.IsNullOrEmpty(x.ApplicantId)).WithMessage("Please set ApplicantId")
         .Must(x => x.Amount > 0).WithMessage("Amount need to be greater than 0")
         .MustAsync(async (applicant, cancellation) => await ApplicantExists(applicant.ApplicantId).ConfigureAwait(false)).WithMessage("Applicant not Exists");
    } 

    private async Task<bool> ApplicantExists(string applicantId)
    {
        if (string.IsNullOrEmpty(applicantId))
            return false;

        var result = await _applicantRepository.GetApplicantAsync(applicantId);

        return result == null? false : true;
    }
}

public class LoanDeductValidation : AbstractValidator<RequestDeduce>
{ 
    public LoanDeductValidation(ApplicantLoan foundLoan)
    {
        RuleFor(x => x).Cascade(CascadeMode.Continue)
          .Must(x => x.Amount > 0).WithMessage("Amount need to be greater than 0")
          .Must(x => foundLoan != null).WithMessage("Loan not found");

        RuleFor(x => x.Amount)
        .Must(amount => foundLoan!.CurrentBalance - amount >= 0)
        .WithMessage("Excess paid amount")
        .When(x => foundLoan != null);
    }
}