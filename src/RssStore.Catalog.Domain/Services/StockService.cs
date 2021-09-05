using RssStore.Catalog.Domain.Events;
using RssStore.Catalog.Domain.Interfaces;
using RssStore.Core.Communication.Mediator;
using RssStore.Core.DomainObjects;
using RssStore.Core.DomainObjects.DTOs;
using RssStore.Core.DomainObjects.Messages.CommonMessages.Notifications;
using System;
using System.Threading.Tasks;

namespace RssStore.Catalog.Domain.Services
{
    public class StockService : IStockService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMediatorHandler _mediatorHandler;
        public StockService(IProductRepository productRepository, IMediatorHandler mediatorHandler)
        {
            _productRepository = productRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> DebitStock(Guid productId, int amount)
        {
            if (!await DebitStockItem(productId, amount)) return false;

            return await _productRepository.UnitOfWork.Commit();
        }

        public async Task<bool> DebitOrderProductList(ProductsOrderList list)
        {
            foreach (var item in list.Items)
            {
                if (!await DebitStockItem(item.Id, item.Amount)) return false;
            }

            return await _productRepository.UnitOfWork.Commit();
        }

        public async Task<bool> DebitStockItem(Guid productId, int amount)
        {
            var product = await _productRepository.GetById(productId);

            if(product == null) return false;

            if (!product.HasStockAmount(amount))
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("Estoque", $"Produto - {product.Name} sem estoque"));
                return false;
            }

            product.DebitStockAmount(amount);

            if(product.StockAmount < 10)
            {
                await _mediatorHandler.PublishDomainEvent(new LowProductStockEvent(product.Id, product.StockAmount));
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

        public async Task<bool> FillOrderProductList(ProductsOrderList list)
        {
            foreach(var item in list.Items)
            {
                if(!await FillProductStock(item.Id, item.Amount)) return false;
            }

            return await _productRepository.UnitOfWork.Commit();
        }

        public void Dispose()
        {
            _productRepository.Dispose();
        }
    }
}
