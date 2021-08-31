using MediatR;
using RssStore.Sales.Application.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RssStore.Sales.Application.EventHandlers
{
    //classe responsável por tratar os eventos referentes a modificação pedido quanto seus itens
    public class OrderItemEventHandler : 
        INotificationHandler<DraftOrderInitializedEvent>,
        INotificationHandler<OrderItemAddedEvent>,
        INotificationHandler<UpdatedOrderEvent>
    {
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
    }
}
