using RssStore.Core.DomainObjects.DTOs;
using System;
using System.Threading.Tasks;

namespace RssStore.Catalog.Domain.Interfaces
{
    public interface IStockService : IDisposable
    {
        Task<bool> DebitStockItem(Guid productId, int amount);
        Task<bool> FillProductStock(Guid productId, int amount);
        Task<bool> DebitStock(Guid productId, int amount);
        Task<bool> DebitOrderProductList(ProductsOrderList list);
    }
}
