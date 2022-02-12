using System;
using System.Linq;
using System.Collections.Generic;
using Application_acceptance_service.App;
using Application_acceptance_service.App.Types;
using Application_acceptance_service.Domain;
using Application_acceptance_service.Infrastructure.Database;
using AutoMapper;

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

        public RequestedCreditDto Get(Guid id)
        {
            var requestedCredit = _applicationContext.RequestedCredits.FirstOrDefault(r => r.Id == id);
            
            return requestedCredit != null ? _mapper.Map<RequestedCreditDto>(requestedCredit) : null;
        }

        public Guid Create(RequestedCreditDto item)
        {
            var requestedCredit = _mapper.Map<RequestedCredit>(item);
            requestedCredit.Id = new Guid();
            _applicationContext.RequestedCredits.Add(requestedCredit);
            _applicationContext.SaveChanges();

            return requestedCredit.Id;
        }

        public void Update(RequestedCreditDto item)
        {
            throw new NotImplementedException();
        }

        public RequestedCreditDto Delete(Guid id)
        {
            var deletedRequestedCredit = _applicationContext.RequestedCredits.FirstOrDefault(r => r.Id == id);
            if (deletedRequestedCredit == null) return null;
            _applicationContext.RequestedCredits.Remove(deletedRequestedCredit);

            return _mapper.Map<RequestedCreditDto>(deletedRequestedCredit);
        }
    }
}