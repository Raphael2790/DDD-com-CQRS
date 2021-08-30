using Microsoft.EntityFrameworkCore;
using RssStore.Catalog.Domain.Entities;
using RssStore.Catalog.Domain.Interfaces;
using RssStore.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RssStore.Catalog.Data.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly CatalogDbContext _context;

        public ProductRepository(CatalogDbContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void AddCategory(Category category) => _context.Categories.Add(category);
        public void AddProduct(Product product) => _context.Products.Add(product);

        public async Task<IEnumerable<Product>> GetAll() =>  
            await _context.Products.AsNoTracking().ToListAsync();

        public async Task<IEnumerable<Product>> GetByCategoryCode(int categoryCode) =>
            await _context.Products.Include(p => p.Category).Where(c => c.Category.Code == categoryCode).ToListAsync();

        public async Task<Product> GetById(Guid id) =>
            await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

        public async Task<IEnumerable<Category>> GetCategories() =>
            await _context.Categories.ToListAsync();

        public void UpdateCategory(Category category) => _context.Categories.Update(category);

        public void UpdateProduct(Product product) => _context.Products.Update(product);

        public void Dispose() => _context?.Dispose();
    }
}
