using System.Threading.Tasks;
using Application_acceptance_service.App.Types;

namespace Application_acceptance_service.App
{
    public interface IScoringService
    {
        public Task<bool?> ScoreApplication(FullApplication fullApplication);
    }
}