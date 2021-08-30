using AutoMapper;
using RssStore.Catalog.Application.Dtos;
using RssStore.Catalog.Domain.Entities;
using RssStore.Core.DomainObjects.ValueObjects;

namespace RssStore.Catalog.Application.AutoMapper
{
    public class DtosToDomainMappingProfile : Profile
    {
        public DtosToDomainMappingProfile()
        {
            //Mapeando para quando temos classes com set privado e só aceitam as mesmas via construtor
            CreateMap<ProductDto, Product>()
                .ConstructUsing(p =>
                new Product(p.Name, p.Description, p.Active, p.Value,
                p.RegisterDate, p.Image, p.CategoryId, new Dimensions(p.Width, p.Height, p.Depth)));

            CreateMap<CategoryDto, Category>()
                .ConstructUsing(c =>
                new Category(c.Name, c.Code));
        }
    }
}
