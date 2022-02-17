using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application_acceptance_service.App.Types;
using Application_acceptance_service.Domain;

namespace Application_acceptance_service.App
{
    /// <summary>
    /// ApplicationManager обрабатывает входящую заявку на кредит
    /// </summary>
    public class ApplicationManager
    {
        private IRepository<ApplicationDto> _applicationRepository;
        private IScoringService _scoringService;
        public ApplicationManager(IRepository<ApplicationDto> applicationRepository, IScoringService scoringService)
        {
            _applicationRepository = applicationRepository;
            _scoringService = scoringService;
        }
        
        /// <summary>
        /// Обработать заявку на кредит
        /// </summary>
        /// <param name="fullApplication">Входящая заявка на кредит</param>
        /// <returns>Id созданной заявки</returns>
        public async Task<Guid> ProcessApplication(FullApplication fullApplication)
        {
            var applicantId = Guid.NewGuid();
            var requestedCreditId = Guid.NewGuid();
            var application = new ApplicationDto
            {
                ApplicationNum = fullApplication.ApplicationNum,
                ApplicationDate = fullApplication.ApplicationDate,
                BranchBank = fullApplication.BranchBank,
                BranchBankAddress = fullApplication.BranchBankAddress,
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
            var applicationId = await _applicationRepository.Create(application);
            var scoringStatus = await _scoringService.ScoreApplication(fullApplication);
            if (scoringStatus == null)
            {
                return applicantId;
            }
            application.ScoringStatus = scoringStatus.Value;
            application.ScoringDate = DateTime.Now;
            _applicationRepository.Update(applicationId, application);
            
            return applicationId;
        }
    }
}