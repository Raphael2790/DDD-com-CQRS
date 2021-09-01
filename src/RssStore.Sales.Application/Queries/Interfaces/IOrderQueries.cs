using RssStore.Sales.Application.Queries.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RssStore.Sales.Application.Queries.Interfaces
{
    public interface IOrderQueries
    {
        Task<CartViewModel> GetClientCart(Guid clientId);
        Task<IEnumerable<OrderViewModel>> GetClientOrders(Guid clientId);
    }
}
