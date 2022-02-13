using System;

namespace Application_acceptance_service.App.Types
{
    /// <summary>
    /// Входящая заявка на кредит
    /// </summary>
    public class FullApplication
    {
        public string ApplicationNum { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string BranchBank { get; set; }
        public string BranchAddress { get; set; }
        public uint CreditManagerId { get; set; }
        public ApplicantDto Applicant { get; set; }
        public RequestedCreditDto RequestedCredit { get; set; }
    }
}