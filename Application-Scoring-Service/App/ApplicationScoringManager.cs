using System;
using Application_Scoring_Service.App.Types;
using Microsoft.Extensions.Logging;

namespace Application_Scoring_Service.App
{
    public class ApplicationScoringManager
    {
        public bool ProcessApplication(FullApplication application)
        {
            var randomScoring = new Random();

            return Convert.ToBoolean(randomScoring.Next(0, 2));
        }
    }
}