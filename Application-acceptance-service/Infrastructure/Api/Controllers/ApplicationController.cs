using System;
using Application_acceptance_service.App;
using Application_acceptance_service.App.Types;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Application_acceptance_service.Infrastructure.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationController : ControllerBase
    {
        private ApplicationManager _applicationManager;
        private ILogger<ApplicationController> _logger;
        private IRepository<ApplicationDto> _applicationRepository;
        private IRepository<ApplicantDto> _applicantRepository;
        private IMapper _mapper;
        public ApplicationController(ApplicationManager applicationManager, ILogger<ApplicationController> logger,
            IRepository<ApplicationDto> applicationRepository, IRepository<ApplicantDto> applicantRepository, IMapper mapper)
        {
            _applicationManager = applicationManager;
            _logger = logger;
            _applicationRepository = applicationRepository;
            _applicantRepository = applicantRepository;
            _mapper = mapper;
        }
        
        [HttpPost]
        [Route("create")]
        public ActionResult<object> Create([FromBody] FullApplication application)
        {
            var applicationId = _applicationManager.ProcessApplication(application);
            
            _logger.LogInformation("Получена заявка на одобрение кредита, Id заявки:{Id}", applicationId);
            
            return new {ApplicationId = applicationId, ApplicationNum = application.ApplicationNum};
        }

        [HttpGet]
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