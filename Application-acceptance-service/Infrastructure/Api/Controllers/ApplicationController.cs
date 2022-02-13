using System;
using System.Threading.Tasks;
using Application_acceptance_service.App;
using Application_acceptance_service.App.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Application_acceptance_service.Infrastructure.Api.Controllers
{
    /// <summary>
    /// Контроллер обработки заявок на кредит
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationController : ControllerBase
    {
        private ApplicationManager _applicationManager;
        private ILogger<ApplicationController> _logger;
        private IRepository<ApplicationDto> _applicationRepository;
        public ApplicationController(ApplicationManager applicationManager, ILogger<ApplicationController> logger,
            IRepository<ApplicationDto> applicationRepository)
        {
            _applicationManager = applicationManager;
            _logger = logger;
            _applicationRepository = applicationRepository;
        }
        
        /// <summary>
        /// Принять заявку на получение кредита
        /// </summary>
        /// <param name="application">Заявка на получение кредита</param>
        /// <returns>Id созданной заявки</returns>
        /// <response code="200">Заявка обработана</response>
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<object>> Create([FromBody] FullApplication application)
        {
            var applicationId = await _applicationManager.ProcessApplication(application);

            _logger.LogInformation("Получена заявка на одобрение кредита, Id заявки:{Id}", applicationId);
            
            return new {ApplicationId = applicationId, application.ApplicationNum};
        }

        /// <summary>
        /// Получить статус по заявке
        /// </summary>
        /// <param name="applicationId">Id заявки на кредит</param>
        /// <returns>Информация о статусе заявки</returns>
        /// <response code="200">Статус о заявке</response>
        /// <response code="404">Заявка не найдена</response>
        [HttpGet]
        [Route("status")]
        public async Task<ActionResult<object>> Status([FromQuery] Guid applicationId)
        {
            var application = await _applicationRepository.Get(applicationId);

            if (application == null)
            {
                return NotFound("Заявка не найдена");
            }
            
            _logger.LogInformation("Запрошен статус по заявке с Id:{Id}", applicationId);

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