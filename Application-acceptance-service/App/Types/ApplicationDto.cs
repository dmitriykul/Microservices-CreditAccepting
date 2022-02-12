using System;
using Application_acceptance_service.Domain;

namespace Application_acceptance_service.App.Types
{
    public class ApplicationDto
    {
        public string ApplicationNum { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string BranchBank { get; set; }
        public string BranchBankAddress { get; set; }
        public uint CreditManagerId { get; set; }
        public bool ScoringStatus { get; set; } = false;
        public DateTime ScoringDate { get; set; } = DateTime.MinValue;
        public Guid ApplicantId { get; set; }
        public Guid RequestedCreditId { get; set; }
        public virtual Applicant Applicant { get; set; }
        public virtual RequestedCredit RequestedCredit { get; set; }
    }
}