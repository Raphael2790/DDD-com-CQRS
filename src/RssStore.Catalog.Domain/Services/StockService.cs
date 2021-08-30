using RssStore.Catalog.Domain.Events;
using RssStore.Catalog.Domain.Interfaces;
using RssStore.Core.Communication.Mediator;
using RssStore.Core.DomainObjects;
using System;
using System.Threading.Tasks;

namespace RssStore.Catalog.Domain.Services
{
    public class StockService : IStockService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMediatorHandler _bus;
        public StockService(IProductRepository productRepository, IMediatorHandler bus)
        {
            _productRepository = productRepository;
            _bus = bus;
        }
        public async Task<bool> DebitProductStock(Guid productId, int amount)
        {
            var product = await _productRepository.GetById(productId);

            if(product == null) throw new DomainException("O produto não existe");
            if (!product.HasStockAmount(amount)) return false;

            product.DebitStockAmount(amount);

            if(product.StockAmount < 10)
            {
                await _bus.PublishEvent(new LowProductStockEvent(product.Id, product.StockAmount));
            }

            _productRepository.UpdateProduct(product);

            return await _productRepository.UnitOfWork.Commit();
        }

        public async Task<bool> FillProductStock(Guid productId, int amount)
        {
            var product = await _productRepository.GetById(productId);

            if (product == null) throw new DomainException("O produto não existe");

            product.FillStockAmount(amount);

            _productRepository.UpdateProduct(product);

            return await _productRepository.UnitOfWork.Commit();
        }

        public void Dispose()
        {
            _productRepository.Dispose();
        }
    }
}
