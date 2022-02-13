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
    public class RequestedCreditRepository : IRepository<RequestedCreditDto>
    {
        private ApplicationContext _applicationContext;
        private IMapper _mapper;
        
        public RequestedCreditRepository(ApplicationContext applicationContext, IMapper mapper)
        {
            _applicationContext = applicationContext;
            _mapper = mapper;
        }
        
        public IEnumerable<RequestedCreditDto> Get()
        {
            var requestedCredits = _applicationContext.RequestedCredits;

            return requestedCredits.Select(r => _mapper.Map<RequestedCreditDto>(r));
        }

        public async Task<RequestedCreditDto> Get(Guid id)
        {
            var requestedCredit = await _applicationContext.RequestedCredits.FirstOrDefaultAsync(r => r.Id == id);
            
            return requestedCredit != null ? _mapper.Map<RequestedCreditDto>(requestedCredit) : null;
        }

        public async Task<Guid> Create(RequestedCreditDto item)
        {
            var requestedCredit = _mapper.Map<RequestedCredit>(item);
            requestedCredit.Id = new Guid();
            await _applicationContext.RequestedCredits.AddAsync(requestedCredit);
            await _applicationContext.SaveChangesAsync();

            return requestedCredit.Id;
        }

        public async void Update(Guid id, RequestedCreditDto item)
        {
            var requestedCredit = await _applicationContext.RequestedCredits.FirstOrDefaultAsync(r => r.Id == id);
            requestedCredit.CreditType = item.CreditType;
            requestedCredit.RequestedAmount = item.RequestedAmount;
            requestedCredit.RequestedCurrency = item.RequestedCurrency;
            requestedCredit.AnnualSalary = item.AnnualSalary;
            requestedCredit.MonthlySalary = item.MonthlySalary;
            requestedCredit.CompanyName = item.CompanyName;
            requestedCredit.Comment = item.Comment;
            
            _applicationContext.RequestedCredits.Update(requestedCredit);
            await _applicationContext.SaveChangesAsync();
        }

        public async Task<RequestedCreditDto> Delete(Guid id)
        {
            var deletedRequestedCredit = await _applicationContext.RequestedCredits.FirstOrDefaultAsync(r => r.Id == id);
            if (deletedRequestedCredit == null) return null;
            _applicationContext.RequestedCredits.Remove(deletedRequestedCredit);

            return _mapper.Map<RequestedCreditDto>(deletedRequestedCredit);
        }
    }
}