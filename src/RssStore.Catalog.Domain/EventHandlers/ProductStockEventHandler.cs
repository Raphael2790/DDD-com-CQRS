using MediatR;
using RssStore.Catalog.Domain.Events;
using RssStore.Catalog.Domain.Interfaces;
using RssStore.Core.Communication.Mediator;
using RssStore.Core.DomainObjects.Messages.CommonMessages.IntegrationEvents;
using System.Threading;
using System.Threading.Tasks;

namespace RssStore.Catalog.Domain.EventHandlers
{
    public class ProductStockEventHandler : 
        INotificationHandler<LowProductStockEvent>,
        INotificationHandler<IniciatedOrderEvent>,
        INotificationHandler<OrderProcessCancelledEvent>
    {
        private readonly IProductRepository _productRepository;
        private readonly IStockService _stockService;
        private readonly IMediatorHandler _mediatorHandler;

        public ProductStockEventHandler(IProductRepository productRepository, IStockService stockService)
        {
            _productRepository = productRepository;
            _stockService = stockService;
        }
        public async Task Handle(LowProductStockEvent message, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetById(message.AggregateId);

            //Uma logica para processar o evento
        }

        public async Task Handle(IniciatedOrderEvent message, CancellationToken cancellationToken)
        {
            var result = await _stockService.DebitOrderProductList(message.ProductsOrderList);

            if (result)
            {
                //quem trata esse evento será o contexto de pagamento
                await _mediatorHandler.PublishEvent(new ConfirmedOrderStockEvent(message.OrderId, message.ClientId, message.Total, message.ProductsOrderList, message.CardName, message.CardNumber, message.CardExpirationDate, message.CardCvv));
            }
            else
            {
                //quem trata esse evento é o contexto de vendas
                await _mediatorHandler.PublishEvent(new RejectedOrderStockEvent(message.OrderId, message.ClientId));
            }
        }

        public async Task Handle(OrderProcessCancelledEvent message, CancellationToken cancellationToken)
        {
            await _stockService.FillOrderProductList(message.ProductOrderList);
        }
    }
}
