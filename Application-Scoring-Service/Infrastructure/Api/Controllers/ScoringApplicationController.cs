using Application_Scoring_Service.App;
using Application_Scoring_Service.App.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Application_Scoring_Service.Infrastructure.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScoringApplicationController : ControllerBase
    {
        private ApplicationScoringManager _applicationScoringManager;
        private ILogger<ScoringApplicationController> _logger;

        public ScoringApplicationController(ApplicationScoringManager applicationScoringManager,
            ILogger<ScoringApplicationController> logger)
        {
            _applicationScoringManager = applicationScoringManager;
            _logger = logger;
        }
        
        [HttpPost]
        [Route("evaluate")]
        public ActionResult<object> Evaluate([FromBody] FullApplication fullApplication)
        {
            var scoringStatus = _applicationScoringManager.ProcessApplication(fullApplication);
            _logger.LogInformation("Запрошен скоринг заявки с номером: {ApplicationNum}", 
                fullApplication.ApplicationNum);
            
            return new { scoringStatus };
        } 
    }
}