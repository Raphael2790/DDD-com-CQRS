using System;
using System.Threading.Tasks;

namespace RssStore.Catalog.Domain.Interfaces
{
    public interface IStockService : IDisposable
    {
        Task<bool> DebitProductStock(Guid productId, int amount);
        Task<bool> FillProductStock(Guid productId, int amount);
    }
}
