using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<ApplicationDto> Get(Guid id)
        {
            var application = await _applicationContext.Applications
                .Include(a => a.Applicant)
                .Include(a => a.RequestedCredit)
                .FirstOrDefaultAsync(a => a.Id == id);
            
            return application != null ? _mapper.Map<ApplicationDto>(application) : null;
        }

        public async Task<Guid> Create(ApplicationDto item)
        {
            var application = _mapper.Map<Application>(item);
            application.Id = new Guid();
            await _applicationContext.Applications.AddAsync(application);
            await _applicationContext.SaveChangesAsync();

            return application.Id;
        }

        public async void Update(Guid id, ApplicationDto item)
        {
            var application = await _applicationContext.Applications
                .FirstOrDefaultAsync(a => a.Id == id);
            application.ApplicationNum = item.ApplicationNum;
            application.ApplicationDate = item.ApplicationDate;
            application.BranchBank = item.BranchBank;
            application.BranchBankAddress = item.BranchBankAddress;
            application.CreditManagerId = item.CreditManagerId;
            application.ScoringStatus = item.ScoringStatus;
            application.ScoringDate = item.ScoringDate;

            _applicationContext.Applications.Update(application);
            await _applicationContext.SaveChangesAsync();
        }

        public async Task<ApplicationDto> Delete(Guid id)
        {
            var deletedApplication = await _applicationContext.Applications.FirstOrDefaultAsync(r => r.Id == id);
            if (deletedApplication == null) return null;
            _applicationContext.Applications.Remove(deletedApplication);

            return _mapper.Map<ApplicationDto>(deletedApplication);
        }
    }
}