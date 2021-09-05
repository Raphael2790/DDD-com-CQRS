using MediatR;
using Microsoft.AspNetCore.Mvc;
using RssStore.Core.Communication.Mediator;
using RssStore.Core.DomainObjects.Messages.CommonMessages.Notifications;
using RssStore.Sales.Application.Queries.Interfaces;
using RssStore.WebApp.MVC.Controllers.Base;
using System.Threading.Tasks;

namespace RssStore.WebApp.MVC.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IOrderQueries _orderQueries;
        public OrderController(INotificationHandler<DomainNotification> notifications,
                               IOrderQueries orderQueries,
                               IMediatorHandler mediatorHandler) : base(notifications, mediatorHandler)
        {
            _orderQueries = orderQueries;
        }

        [HttpGet("meus-pedidos")]
        public async Task<IActionResult> Index()
        {
            return View(await _orderQueries.GetClientOrders(ClientId));
        }
    }
}
