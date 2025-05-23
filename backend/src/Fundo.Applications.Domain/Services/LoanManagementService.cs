﻿using FluentValidation.Results;
using Fundo.Applications.Domain.Common.Enums;
using Fundo.Applications.Domain.Extentions;
using Fundo.Applications.Domain.Interfaces;
using Fundo.Applications.Domain.Models;
using Fundo.Applications.Domain.Validations;
using Fundo.Applications.Repository.Entity;
using Fundo.Applications.Repository.Interface;

namespace Fundo.Applications.Domain.Services;

public class LoanManagementService : ILoanManagementService
{
    private readonly ILoanRepository _loanRepository;
    private readonly IApplicantRepository _applicantRepository;
    private readonly IHistoryRepository _historyRepository;

    public LoanManagementService(ILoanRepository loanRepository, 
        IApplicantRepository applicantRepository, 
        IHistoryRepository historyRepository)
    {
        _loanRepository = loanRepository;
        _applicantRepository = applicantRepository;
        _historyRepository = historyRepository;
    }

    public async Task<ValidationResult> InsertLoanAsync(RequestLoan requestLoan)
    {
        LoanValidation validation = new(_applicantRepository);

        var validationResult = await validation.ValidateAsync(requestLoan);

        if (!validationResult.IsValid)
            return validationResult;

        requestLoan.DateInsert = DateTime.UtcNow;
        requestLoan.LoanId = BaseEntity.GenerateId();

        await _loanRepository.InsertLoanAsync(requestLoan.ToEntity());
        return validationResult;
    }

    public async Task<ValidationResult> DeductLoanAsync(string loanId, RequestDeduce requestDeduce)
    {
        var foundLoan = await _loanRepository.GetLoanDetailsAsync(loanId);
        foundLoan ??= new ApplicantLoan();

        var validation = new LoanDeductValidation(foundLoan);
        var validationResult = await validation.ValidateAsync(requestDeduce);

        if (!validationResult.IsValid)
            return validationResult;

        Loan updateLoan = LoanExtensions.ToEntityLoan(foundLoan!);
        updateLoan!.DateUpdate = DateTime.UtcNow;
        updateLoan.CurrentBalance = updateLoan.CurrentBalance - requestDeduce.Amount;
        updateLoan.Status = updateLoan.CurrentBalance == 0 ? (int)StatusLoan.Paid : (int)StatusLoan.Active;

        HistoryDeduce history = new HistoryDeduce()
        { 
            Amount = requestDeduce.Amount,
            CurrentBalance = updateLoan.CurrentBalance,
            Status = updateLoan.Status,
            LoanId = foundLoan.LoanId
        };
        await _historyRepository.InsertHistoryLoanAsync(history);

        await _loanRepository.UpdateLoanAsync(updateLoan);
        return validationResult;
    }

    public async Task<ApplicantLoan?> GetLoanDetailsAsync(string loanId)
    {
        return await _loanRepository.GetLoanDetailsAsync(loanId);
    }

    public async Task<List<ApplicantLoan>> GetLoansAsync()
    {
        return await _loanRepository.GetLoansAsync() ?? new List<ApplicantLoan>();
    }
}

