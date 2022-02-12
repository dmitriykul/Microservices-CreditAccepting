using System;
using System.Collections.Generic;
using System.Linq;
using Application_acceptance_service.App;
using Application_acceptance_service.App.Types;
using Application_acceptance_service.Domain;
using Application_acceptance_service.Infrastructure.Database;
using AutoMapper;

namespace Application_acceptance_service.Infrastructure.Repository
{
    public class ApplicantRepository : IRepository<ApplicantDto>
    {
        private ApplicationContext _applicationContext;
        private IMapper _mapper;
        
        public ApplicantRepository(ApplicationContext applicationContext, IMapper mapper)
        {
            _applicationContext = applicationContext;
            _mapper = mapper;
        }
        
        public IEnumerable<ApplicantDto> Get()
        {
            var applicants = _applicationContext.Applicants;

            return applicants.Select(a => _mapper.Map<ApplicantDto>(a));
        }

        public ApplicantDto Get(Guid id)
        {
            var applicant = _applicationContext.Applicants.FirstOrDefault(a => a.Id == id);
            
            return applicant != null ? _mapper.Map<ApplicantDto>(applicant) : null;
        }

        public Guid Create(ApplicantDto item)
        {
            var applicant = _mapper.Map<Applicant>(item);
            applicant.Id = new Guid();
            _applicationContext.Applicants.Add(applicant);
            _applicationContext.SaveChanges();

            return applicant.Id;
        }

        public void Update(Guid id, ApplicantDto item)
        {
            var applicant = _applicationContext.Applicants.FirstOrDefault(a => a.Id == id);
            applicant.FirstName = item.FirstName;
            applicant.MiddleName = item.MiddleName;
            applicant.LastName = item.LastName;
            applicant.DateBirth = item.DateBirth;
            applicant.CityBirth = item.CityBirth;
            applicant.AddressBirth = item.AddressBirth;
            applicant.AddressCurrent = item.AddressCurrent;
            applicant.INN = item.INN;
            applicant.SNILS = item.SNILS;
            applicant.PassportNum = applicant.PassportNum;
            
            _applicationContext.Applicants.Update(applicant);
            _applicationContext.SaveChanges();
        }

        public ApplicantDto Delete(Guid id)
        {
            var deletedApplicant = _applicationContext.Applicants.FirstOrDefault(a => a.Id == id);
            if (deletedApplicant == null) return null;
            _applicationContext.Applicants.Remove(deletedApplicant);

            return _mapper.Map<ApplicantDto>(deletedApplicant);
        }
    }
}