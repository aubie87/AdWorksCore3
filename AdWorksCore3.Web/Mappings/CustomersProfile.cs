using AdWorksCore3.Core.Entities;
using AdWorksCore3.Web.ViewModels;
using AutoMapper;

namespace AdWorksCore3.Web.Mappings
{
    public class CustomersProfile : Profile
    {
        public CustomersProfile()
        {
            CreateMap<Customer, CustomerGetViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.LastModified, opt => opt.MapFrom(src => src.ModifiedDate))
                .ForMember(dest => dest.Addresses, opt => opt.MapFrom(src => src.CustomerAddress));

            CreateMap<CustomerUpdateViewModel, Customer>();
        }
    }
}
