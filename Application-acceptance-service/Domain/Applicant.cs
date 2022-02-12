using System;
using System.Collections.Generic;

namespace Application_acceptance_service.Domain
{
    public class Applicant
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DateBirth { get; set; }
        public string CityBirth { get; set; }
        public string AddressBirth { get; set; }
        public string AddressCurrent { get; set; }
        public string INN { get; set; }
        public string SNILS { get; set; }
        public string PassportNum { get; set; }

        public virtual List<Application> Applications { get; set; } = new List<Application>();
    }
}