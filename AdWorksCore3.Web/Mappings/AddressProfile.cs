using AdWorksCore3.Core.Entities;
using AdWorksCore3.Web.ViewModels;
using AutoMapper;

namespace AdWorksCore3.Web.Mappings
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<CustomerAddress, AddressGetViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AddressId))
                .ForMember(dest => dest.AddressLine1, opt => opt.MapFrom(src => src.Address.AddressLine1))
                .ForMember(dest => dest.AddressLine2, opt => opt.MapFrom(src => src.Address.AddressLine2))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.StateProvince, opt => opt.MapFrom(src => src.Address.StateProvince))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Address.CountryRegion))
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.Address.PostalCode))
                .ForMember(dest => dest.LastModified, opt => opt.MapFrom(src => src.Address.ModifiedDate));
        }
    }
}
