using System;
using System.Collections.Generic;
using Application_acceptance_service.App.Types;
using Application_acceptance_service.Domain;

namespace Application_acceptance_service.App
{
    public class ApplicationManager
    {
        private IRepository<ApplicationDto> _applicationRepository;
        public ApplicationManager(IRepository<ApplicationDto> applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }
        public Guid ProcessApplication(FullApplication fullApplication)
        {
            var applicantId = new Guid();
            var requestedCreditId = new Guid();
            var application = new ApplicationDto
            {
                ApplicationNum = fullApplication.ApplicationNum,
                ApplicationDate = fullApplication.ApplicationDate,
                BranchBank = fullApplication.BranchBank,
                BranchBankAddress = fullApplication.BranchAddress,
                CreditManagerId = fullApplication.CreditManagerId,
                ApplicantId = applicantId,
                RequestedCreditId = requestedCreditId,
                Applicant = new Applicant
                {
                    Id = applicantId,
                    FirstName = fullApplication.Applicant.FirstName,
                    MiddleName = fullApplication.Applicant.MiddleName,
                    LastName = fullApplication.Applicant.LastName,
                    DateBirth = fullApplication.Applicant.DateBirth,
                    CityBirth = fullApplication.Applicant.CityBirth,
                    AddressBirth = fullApplication.Applicant.AddressBirth,
                    AddressCurrent = fullApplication.Applicant.AddressCurrent,
                    INN = fullApplication.Applicant.INN,
                    SNILS = fullApplication.Applicant.SNILS,
                    PassportNum = fullApplication.Applicant.PassportNum,
                    Applications = new List<Application>()
                },
                RequestedCredit = new RequestedCredit
                {
                    Id = requestedCreditId,
                    CreditType = fullApplication.RequestedCredit.CreditType,
                    RequestedAmount = fullApplication.RequestedCredit.RequestedAmount,
                    RequestedCurrency = fullApplication.RequestedCredit.RequestedCurrency,
                    AnnualSalary = fullApplication.RequestedCredit.AnnualSalary,
                    MonthlySalary = fullApplication.RequestedCredit.MonthlySalary,
                    CompanyName = fullApplication.RequestedCredit.CompanyName,
                    Comment = fullApplication.RequestedCredit.Comment,
                    Applications = new List<Application>()
                }
            };
            var applicationId = _applicationRepository.Create(application);
            
            return applicationId;
        }
    }
}