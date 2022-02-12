using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application_acceptance_service.App.Types;
using Application_acceptance_service.Domain;
using Application_acceptance_service.Infrastructure;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;

namespace Application_acceptance_service.App
{
    public class ApplicationManager
    {
        private IRepository<ApplicationDto> _applicationRepository;
        private IOptions<ApplicationAcceptanceOptions> _options;
        public ApplicationManager(IRepository<ApplicationDto> applicationRepository, IOptions<ApplicationAcceptanceOptions> options)
        {
            _applicationRepository = applicationRepository;
            _options = options;
        }
        public async Task<Guid> ProcessApplication(FullApplication fullApplication)
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
            
            var client = new RestClient(_options.Value.ApplicationScoringServiceUrl);
            var request = new RestRequest()
                .AddJsonBody(fullApplication);
            var response = await client.PostAsync(request);
            if (response.Content == null)
            {
                return applicationId;
            }
            
            var scoringStatus = JsonConvert.DeserializeObject<ScoringServiceResponse>(response.Content).ScoringStatus;
            application.ScoringStatus = scoringStatus;
            _applicationRepository.Update(applicationId, application);
            
            return applicationId;
        }
    }
}