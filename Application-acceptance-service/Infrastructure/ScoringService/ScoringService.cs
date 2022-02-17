using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Application_acceptance_service.App;
using Application_acceptance_service.App.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Application_acceptance_service.Infrastructure.ScoringService
{
    /// <summary>
    /// Сервис для обращения к сервису скоринга заявки
    /// </summary>
    public class ScoringService : IScoringService
    {
        private IOptions<ApplicationAcceptanceOptions> _options;
        private IHttpClientFactory _httpClientFactory;
        private ILogger<ScoringService> _logger;

        public ScoringService(IOptions<ApplicationAcceptanceOptions> options, IHttpClientFactory httpClientFactory,
            ILogger<ScoringService> logger)
        {
            _options = options;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        /// <summary>
        /// Обратиться к сервису для скоринга заявки
        /// </summary>
        /// <param name="fullApplication">Входящая заявка</param>
        /// <returns>Результат скоринга, true, false</returns>
        public async Task<bool?> ScoreApplication(FullApplication fullApplication)
        {
            var client = _httpClientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(fullApplication);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(_options.Value.ApplicationScoringServiceUrl, data);
            if (response.Content == null)
            {
                _logger.LogWarning("Не получен ответ от сервиса скоринга заявка, номер заявки: {Num}", fullApplication.ApplicationNum);
                
                return null;
            }
            var responseResult = await response.Content.ReadAsStringAsync();
            var scoringStatus = JsonConvert.DeserializeObject<ScoringServiceResponse>(responseResult)!.ScoringStatus;
            _logger.LogInformation("Запрошен скоринг заявки у сервиса скоринга заявки, номер заявки: {Num}", fullApplication.ApplicationNum);
            
            return scoringStatus;
        }
    }
}