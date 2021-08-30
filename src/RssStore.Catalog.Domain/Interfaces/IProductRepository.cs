using RssStore.Catalog.Domain.Entities;
using RssStore.Core.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RssStore.Catalog.Domain.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetAll();
        Task<Product> GetById(Guid id);
        Task<IEnumerable<Product>> GetByCategoryCode(int categoryCode);
        Task<IEnumerable<Category>> GetCategories();

        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void AddCategory(Category category);
        void UpdateCategory(Category category);
    }
}
