using System;
using System.Collections.Generic;

namespace Application_acceptance_service.Domain
{
    public class RequestedCredit
    {
        public Guid Id { get; set; }
        public int CreditType { get; set; }
        public decimal RequestedAmount { get; set; }
        public string RequestedCurrency { get; set; }
        public decimal AnnualSalary { get; set; }
        public decimal MonthlySalary { get; set; }
        public string CompanyName { get; set; }
        public string Comment { get; set; }

        public virtual List<Application> Applications { get; set; } = new List<Application>();
    }
}