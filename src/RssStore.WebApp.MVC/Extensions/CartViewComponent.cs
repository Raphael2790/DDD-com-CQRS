using Microsoft.AspNetCore.Mvc;
using RssStore.Sales.Application.Queries.Interfaces;
using System;
using System.Threading.Tasks;

namespace RssStore.WebApp.MVC.Extensions
{
    public class CartViewComponent : ViewComponent
    {
        private readonly IOrderQueries _orderQueries;
        protected Guid ClientId = Guid.Parse("59876ad0-b3fa-4979-8ca8-ac9a76705547");
        public CartViewComponent(IOrderQueries orderQueries)
        {
            _orderQueries = orderQueries;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cart = await _orderQueries.GetClientCart(ClientId);

            var itemsCount = cart?.CartItems.Count ?? 0;

            return View(itemsCount);
        }
    }
}
