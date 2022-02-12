using System;
using System.Linq;
using System.Collections.Generic;
using Application_acceptance_service.App;
using Application_acceptance_service.App.Types;
using Application_acceptance_service.Domain;
using Application_acceptance_service.Infrastructure.Database;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Application_acceptance_service.Infrastructure.Repository
{
    public class ApplicationRepository : IRepository<ApplicationDto>
    {
        private ApplicationContext _applicationContext;
        private IMapper _mapper;
        
        public ApplicationRepository(ApplicationContext applicationContext, IMapper mapper)
        {
            _applicationContext = applicationContext;
            _mapper = mapper;
        }
        
        public IEnumerable<ApplicationDto> Get()
        {
            var applications = _applicationContext.Applications
                .Include(a => a.Applicant)
                .Include(a => a.RequestedCredit);

            return applications.Select(a => _mapper.Map<ApplicationDto>(a));
        }

        public ApplicationDto Get(Guid id)
        {
            var application = _applicationContext.Applications
                .Include(a => a.Applicant)
                .Include(a => a.RequestedCredit)
                .FirstOrDefault(a => a.Id == id);
            
            return application != null ? _mapper.Map<ApplicationDto>(application) : null;
        }

        public Guid Create(ApplicationDto item)
        {
            var application = _mapper.Map<Application>(item);
            application.Id = new Guid();
            _applicationContext.Applications.Add(application);
            _applicationContext.SaveChanges();

            return application.Id;
        }

        public void Update(Guid id, ApplicationDto item)
        {
            var application = _applicationContext.Applications
                .FirstOrDefault(a => a.Id == id);
            application.ApplicationNum = item.ApplicationNum;
            application.ApplicationDate = item.ApplicationDate;
            application.BranchBank = item.BranchBank;
            application.BranchBankAddress = item.BranchBankAddress;
            application.CreditManagerId = item.CreditManagerId;
            application.ScoringStatus = item.ScoringStatus;
            application.ScoringDate = item.ScoringDate;

            _applicationContext.Applications.Update(application);
            _applicationContext.SaveChanges();
        }

        public ApplicationDto Delete(Guid id)
        {
            var deletedApplication = _applicationContext.Applications.FirstOrDefault(r => r.Id == id);
            if (deletedApplication == null) return null;
            _applicationContext.Applications.Remove(deletedApplication);

            return _mapper.Map<ApplicationDto>(deletedApplication);
        }
    }
}