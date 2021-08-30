using RssStore.Catalog.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RssStore.Catalog.Application.Interfaces
{
    public interface IProductAppService : IDisposable
    {
        Task<IEnumerable<ProductDto>> GetByCategory(int categoryCode);
        Task<ProductDto> GetById(Guid id);
        Task<IEnumerable<ProductDto>> GetAll();
        Task<IEnumerable<CategoryDto>> GetCategories();

        Task AddProduct(ProductDto productDto);
        Task UpdateProduct(ProductDto productDto);

        Task<ProductDto> DebitStockAmount(Guid id, int amount);
        Task<ProductDto> FillStockAmount(Guid id, int amount);
    }
}
