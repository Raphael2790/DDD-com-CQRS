using AutoMapper;
using RssStore.Catalog.Application.Dtos;
using RssStore.Catalog.Domain.Entities;

namespace RssStore.Catalog.Application.AutoMapper
{
    public class DomainToDtoMappingProfile : Profile
    {
        public DomainToDtoMappingProfile()
        {
            //Mapeando value objects
            CreateMap<Product, ProductDto>()
                .ForMember(p => p.Width, o => o.MapFrom(s => s.Dimensions.Width))
                .ForMember(p => p.Height, o => o.MapFrom(s => s.Dimensions.Height))
                .ForMember(p => p.Depth, o => o.MapFrom(s => s.Dimensions.Depth));

            CreateMap<Category, CategoryDto>();
        }
    }
}
