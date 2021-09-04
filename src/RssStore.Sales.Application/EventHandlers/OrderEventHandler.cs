using MediatR;
using RssStore.Core.Communication.Mediator;
using RssStore.Core.DomainObjects.Messages.CommonMessages.IntegrationEvents;
using RssStore.Sales.Application.Commands;
using RssStore.Sales.Application.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RssStore.Sales.Application.EventHandlers
{
    //classe responsável por tratar os eventos referentes a modificação pedido quanto seus itens
    public class OrderEventHandler : 
        INotificationHandler<DraftOrderInitializedEvent>,
        INotificationHandler<OrderItemAddedEvent>,
        INotificationHandler<UpdatedOrderEvent>,
        INotificationHandler<RejectedOrderStockEvent>,
        INotificationHandler<AuthorizedPaymentEvent>,
        INotificationHandler<UnauthorizedPaymentEvent>
    {
        private readonly IMediatorHandler _mediatorHandler;
        public OrderEventHandler(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        public Task Handle(DraftOrderInitializedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(OrderItemAddedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(UpdatedOrderEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task Handle(RejectedOrderStockEvent message, CancellationToken cancellationToken)
        {
            await _mediatorHandler.SendCommand(new CancelOrderProcessCommand(message.OrderId, message.ClientId));
        }

        public async Task Handle(UnauthorizedPaymentEvent message, CancellationToken cancellationToken)
        {
            await _mediatorHandler.SendCommand(new CancelOrderProcessReturnStockCommand(message.OrderId, message.ClientId));
        }

        public async Task Handle(AuthorizedPaymentEvent message, CancellationToken cancellationToken)
        {
            await _mediatorHandler.SendCommand(new FinishOrderCommand(message.OrderId, message.ClientId));
        }
    }
}
