using Microsoft.AspNetCore.Mvc;
using RssStore.Catalog.Application.Interfaces;
using RssStore.Sales.Application.Commands;
using System;
using System.Threading.Tasks;
using RssStore.WebApp.MVC.Controllers.Base;
using RssStore.Core.Communication.Mediator;
using MediatR;
using RssStore.Core.DomainObjects.Messages.CommonMessages.Notifications;

namespace RssStore.WebApp.MVC.Controllers
{
    public class ShoppingCartController : BaseController
    {
        private readonly IProductAppService _productAppService;
        private readonly IMediatorHandler _mediatorHandler;
        public ShoppingCartController(INotificationHandler<DomainNotification> notifications, 
                                      IProductAppService productAppService, 
                                      IMediatorHandler mediatorHandler) : base(notifications, mediatorHandler)
        {
            _productAppService = productAppService;
            _mediatorHandler = mediatorHandler;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("meu-carrinho")]
        public async Task<IActionResult> AddItem(Guid id, int amount)
        {
            var product = await _productAppService.GetById(id);
            if (product == null) return BadRequest();

            if(product.StockAmount < amount)
            {
                TempData["Erro"] = "Produto com estoque insuficiente";
                return RedirectToAction("CatalogDetail", "Catalog", new { id });
            }

            var command = new AddOrderItemCommand(ClientId, product.Id, product.Name, amount, product.Value);
            await _mediatorHandler.SendCommand(command);

            if (ValidOperation())
            {
                RedirectToAction(nameof(Index), nameof(CatalogController));
            }

            TempData["Erros"] = GetErrorsMessages();
            return RedirectToAction("CatalogDetail", nameof(CatalogController), new { id });
        }
    }
}
