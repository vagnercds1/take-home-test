using FluentValidation.Results;
using Fundo.Applications.Domain.Models;
using Fundo.Applications.Repository.Entity;

namespace Fundo.Applications.Domain.Interfaces
{
    public interface ILoanManagementService
    {
        Task<ValidationResult> InsertLoanAsync(RequestLoan requestLoan);

        Task<ValidationResult> DeductLoanAsync(string loanId, RequestDeduce requestDeduce);

        Task<ApplicantLoan?> GetLoanDetailsAsync(string loanId);

        Task<List<ApplicantLoan>> GetLoansAsync(); 
    }
}
