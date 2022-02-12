using Application_acceptance_service.App.Types;
using Application_acceptance_service.Domain;
using AutoMapper;

namespace Application_acceptance_service.Infrastructure.Automapper
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<Applicant, ApplicantDto>().ReverseMap();
            CreateMap<Application, FullApplication>().ReverseMap();
            CreateMap<RequestedCredit, RequestedCreditDto>().ReverseMap();
            CreateMap<Application, ApplicationDto>().ReverseMap();
            CreateMap<FullApplication, ApplicantDto>().ReverseMap();
        }
    }
}