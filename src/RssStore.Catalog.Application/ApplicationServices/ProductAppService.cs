using AutoMapper;
using RssStore.Catalog.Application.Dtos;
using RssStore.Catalog.Application.Interfaces;
using RssStore.Catalog.Domain.Entities;
using RssStore.Catalog.Domain.Interfaces;
using RssStore.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RssStore.Catalog.Application.ApplicationServices
{
    public class ProductAppService : IProductAppService
    {
        private readonly IProductRepository _productRepository;
        private readonly IStockService _stockService;
        private readonly IMapper _mapper;

        public ProductAppService(IProductRepository productRepository, IMapper mapper, IStockService stockService)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _stockService = stockService;
        }
        public async Task<ProductDto> GetById(Guid id) => 
            _mapper.Map<ProductDto>(await _productRepository.GetById(id));

        public async Task<IEnumerable<CategoryDto>> GetCategories() => 
            _mapper.Map<IEnumerable<CategoryDto>>(await _productRepository.GetCategories());

        public async Task<IEnumerable<ProductDto>> GetByCategory(int categoryCode) =>
            _mapper.Map<IEnumerable<ProductDto>>(await _productRepository.GetByCategoryCode(categoryCode));

        public async Task<IEnumerable<ProductDto>> GetAll() => 
            _mapper.Map<IEnumerable<ProductDto>>(await _productRepository.GetAll());

        public async Task AddProduct(ProductDto productDto) 
        {
            _productRepository.AddProduct(_mapper.Map<Product>(productDto));
            await _productRepository.UnitOfWork.Commit();
        }

        public async Task UpdateProduct(ProductDto productDto)
        {
            _productRepository.UpdateProduct(_mapper.Map<Product>(productDto));
            await _productRepository.UnitOfWork.Commit();
        }

        public async Task<ProductDto> DebitStockAmount(Guid id, int amount)
        {
            if (!(await _stockService.DebitStockItem(id, amount)))
                throw new DomainException("Falha ao debitar do estoque");

            return _mapper.Map<ProductDto>(await _productRepository.GetById(id));
        }

        public async Task<ProductDto> FillStockAmount(Guid id, int amount)
        {
            if (!(await _stockService.FillProductStock(id, amount)))
                throw new DomainException("falha ao repor estoque");

            return _mapper.Map<ProductDto>(await _productRepository.GetById(id));
        }

        public void Dispose()
        {
            _productRepository?.Dispose();
            _stockService?.Dispose();
        }
    }
}
