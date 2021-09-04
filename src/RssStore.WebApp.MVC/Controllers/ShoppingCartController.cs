using Microsoft.AspNetCore.Mvc;
using RssStore.Catalog.Application.Interfaces;
using RssStore.Sales.Application.Commands;
using System;
using System.Threading.Tasks;
using RssStore.WebApp.MVC.Controllers.Base;
using RssStore.Core.Communication.Mediator;
using MediatR;
using RssStore.Core.DomainObjects.Messages.CommonMessages.Notifications;
using RssStore.Sales.Application.Queries.Interfaces;
using RssStore.Sales.Application.Queries.ViewModels;

namespace RssStore.WebApp.MVC.Controllers
{
    public class ShoppingCartController : BaseController
    {
        private readonly IProductAppService _productAppService;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IOrderQueries _orderQueries;
        public ShoppingCartController(INotificationHandler<DomainNotification> notifications,
                                      IProductAppService productAppService,
                                      IMediatorHandler mediatorHandler, IOrderQueries orderQueries) : base(notifications, mediatorHandler)
        {
            _productAppService = productAppService;
            _mediatorHandler = mediatorHandler;
            _orderQueries = orderQueries;
        }

        [HttpGet("meu-carrinho")]
        public async Task<IActionResult> Index()
        {
            return View(await _orderQueries.GetClientOrders(ClientId));
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

        [HttpPost("remover-item")]
        public async Task<IActionResult> RemoveItem(Guid productId)
        {
            var product = await _productAppService.GetById(productId);

            if (product is null) return BadRequest();

            var command = new RemoveOrderItemCommand(ClientId, productId);

            await _mediatorHandler.SendCommand(command);

            if (ValidOperation())
            {
                return RedirectToAction(nameof(Index));
            }

            return View(nameof(Index), await _orderQueries.GetClientCart(ClientId));
        }

        [HttpPost("atualizar-item")]
        public async Task<IActionResult> UpdateItem(Guid productId, int amount)
        {
            var product = await _productAppService.GetById(productId);

            if (product is null) return BadRequest();

            var command = new UpdateOrderItemCommand(ClientId, productId, amount);

            await _mediatorHandler.SendCommand(command);

            if (ValidOperation())
            {
                RedirectToAction(nameof(Index));
            }

            return View(nameof(Index), await _orderQueries.GetClientCart(ClientId));
        }

        [HttpPost("aplicar-voucher")]
        public async Task<IActionResult> ApplyVoucher(string voucherCode)
        {
            var command = new ApplyVoucherOrderCommand(ClientId, voucherCode);
            await _mediatorHandler.SendCommand(command);

            if (ValidOperation())
            {
                return RedirectToAction(nameof(Index));
            }

            return View(nameof(Index), await _orderQueries.GetClientCart(ClientId));
        }

        [HttpGet("resumo-da-compra")]
        public async Task<IActionResult> PurchaseResume()
        {
            return View(await _orderQueries.GetClientCart(ClientId));
        }

        [HttpPost("iniciar-pedido")]
        public async Task<IActionResult> IniciateOrder(CartViewModel cartViewModel)
        {
            var cart = await _orderQueries.GetClientCart(ClientId);

            var command = new IniciateOrderCommand(cart.OrderId, ClientId, cart.TotalValue, cartViewModel.Payment.CardName, cartViewModel.Payment.CardNumber,
                cartViewModel.Payment.ExpirationCardDate, cartViewModel.Payment.CvvCardCode);

            await _mediatorHandler.SendCommand(command);

            if (ValidOperation())
            {
                return RedirectToAction("Index", "Order");
            }

            return View("PurchaseResume", await _orderQueries.GetClientCart(ClientId));
        }
    }
}
