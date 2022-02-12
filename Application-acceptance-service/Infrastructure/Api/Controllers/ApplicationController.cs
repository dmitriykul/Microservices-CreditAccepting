using System;
using System.Text.Json;
using System.Threading.Tasks;
using Application_acceptance_service.App;
using Application_acceptance_service.App.Types;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;

namespace Application_acceptance_service.Infrastructure.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationController : ControllerBase
    {
        private ApplicationManager _applicationManager;
        private ILogger<ApplicationController> _logger;
        private IRepository<ApplicationDto> _applicationRepository;
        private IOptions<ApplicationAcceptanceOptions> _options;
        public ApplicationController(ApplicationManager applicationManager, ILogger<ApplicationController> logger,
            IRepository<ApplicationDto> applicationRepository, IOptions<ApplicationAcceptanceOptions> options)
        {
            _applicationManager = applicationManager;
            _logger = logger;
            _applicationRepository = applicationRepository;
            _options = options;
        }
        
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<object>> Create([FromBody] FullApplication application)
        {
            var applicationId = await _applicationManager.ProcessApplication(application);

            _logger.LogInformation("Получена заявка на одобрение кредита, Id заявки:{Id}", applicationId);
            
            return new {ApplicationId = applicationId, ApplicationNum = application.ApplicationNum};
        }

        [HttpGet]
        [Route("status")]
        public ActionResult<object> Status([FromQuery] Guid applicationId)
        {
            var application = _applicationRepository.Get(applicationId);

            return new
            {
                Id = applicationId,
                application.ApplicationNum,
                application.ApplicationDate,
                application.BranchBank,
                application.BranchBankAddress,
                application.CreditManagerId,
                Applicant = new
                {
                    application.Applicant.FirstName,
                    application.Applicant.MiddleName,
                    application.Applicant.LastName
                },
                RequestedCredit = new
                {
                    application.RequestedCredit.CreditType,
                    application.RequestedCredit.RequestedAmount,
                    application.RequestedCredit.RequestedCurrency
                },
                application.ScoringStatus,
                application.ScoringDate
            };
        }
    }
}