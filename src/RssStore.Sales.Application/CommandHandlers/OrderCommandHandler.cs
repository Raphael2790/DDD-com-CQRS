using MediatR;
using RssStore.Core.Communication.Mediator;
using RssStore.Core.DomainObjects.DTOs;
using RssStore.Core.DomainObjects.Messages;
using RssStore.Core.DomainObjects.Messages.CommonMessages.IntegrationEvents;
using RssStore.Core.DomainObjects.Messages.CommonMessages.Notifications;
using RssStore.Core.Extensions;
using RssStore.Sales.Application.Commands;
using RssStore.Sales.Application.Events;
using RssStore.Sales.Domain.Entities;
using RssStore.Sales.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RssStore.Sales.Application.CommandHandlers
{
    //Todos comandos lançados referentes a uma entidade podem ser agrupados ou separados 
    public class OrderCommandHandler : 
        IRequestHandler<AddOrderItemCommand, bool>,
        IRequestHandler<ApplyVoucherOrderCommand, bool>,
        IRequestHandler<RemoveOrderItemCommand, bool>,
        IRequestHandler<UpdateOrderItemCommand, bool>,
        IRequestHandler<IniciateOrderCommand, bool>,
        IRequestHandler<FinishOrderCommand, bool>,
        IRequestHandler<CancelOrderProcessReturnStockCommand, bool>,
        IRequestHandler<CancelOrderProcessCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMediatorHandler _mediatorHandler;
        public OrderCommandHandler(IOrderRepository orderRepository, IMediatorHandler mediatorHandler)
        {
            _orderRepository = orderRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(AddOrderItemCommand message, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(message)) return false;

            var order = await _orderRepository.GetDraftOrderByClientId(message.ClientId);
            var orderItem = new OrderItem(message.ProductId, message.ProductName, message.Amount, message.UnitValue);

            if(order == null)
            {
                order = Order.OrderFactory.NewDrafOrder(message.ClientId);
                order.AddItem(orderItem);

                _orderRepository.AddOrder(order);
                order.AddEvent(new DraftOrderInitializedEvent(message.ClientId, message.AggregateId));
            }
            else
            {
                var existingOrderItem = order.OrderItemExists(orderItem);
                order.AddItem(orderItem);

                if (existingOrderItem)
                {
                    _orderRepository.UpdateOrderItem(order.OrderItems.FirstOrDefault(o => o.ProductId == orderItem.ProductId));
                }
                else
                {
                    _orderRepository.AddOrderItem(orderItem);
                }

                order.AddEvent(new UpdatedOrderEvent(message.ClientId, order.Id, order.TotalValue));
            }

            order.AddEvent(new OrderItemAddedEvent(message.ClientId, order.Id, message.ProductId, message.UnitValue, message.Amount, message.ProductName));

            return await _orderRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(UpdateOrderItemCommand message, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(message)) return false;

            var order = await _orderRepository.GetDraftOrderByClientId(message.ClientId);

            if(order is null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("pedido", "Pedido não encotrado"));
                return false;
            }

            var orderItem = await _orderRepository.GetOrderItemByOrderId(order.Id, message.ProductId);

            if (!order.OrderItemExists(orderItem))
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("pedido", "Item do pedido não encontrado"));
                return false;
            }

            order.UpdateItemAmount(orderItem, message.Amount);

            order.AddEvent(new UpdatedOrderEvent(order.ClientId, order.Id, order.TotalValue));
            order.AddEvent(new ProductOrderUpdatedEvent(message.ClientId, order.Id, message.ProductId, message.Amount));

            _orderRepository.UpdateOrderItem(orderItem);
            _orderRepository.UpdateOrder(order);

            return await _orderRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(RemoveOrderItemCommand message, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(message)) return false;

            var order = await _orderRepository.GetDraftOrderByClientId(message.ClientId);

            if (order is null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("pedido", "Pedido não encotrado"));
                return false;
            }

            var orderItem = await _orderRepository.GetOrderItemByOrderId(order.Id, message.ProductId);

            if (!order.OrderItemExists(orderItem))
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("pedido", "Item do pedido não encontrado"));
                return false;
            }

            order.RemoveItem(orderItem);

            order.AddEvent(new UpdatedOrderEvent(order.ClientId, order.Id, order.TotalValue));
            order.AddEvent(new RemovedProductOrderEvent(message.ClientId, order.Id, message.ProductId));

            _orderRepository.RemoveOrderItem(orderItem);
            _orderRepository.UpdateOrder(order);

            return await _orderRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(ApplyVoucherOrderCommand message, CancellationToken cancellationToken)
        {

            if (!ValidateCommand(message)) return false;

            var order = await _orderRepository.GetDraftOrderByClientId(message.ClientId);

            if (order is null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("pedido", "Pedido não encotrado"));
                return false;
            }

            var voucher = await _orderRepository.GetVoucherByCode(message.VoucherCode);

            if(voucher is null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("pedido", "Voucher não encontrado!"));
                return false;
            }

            var applyVoucherValidation = order.ApplyVoucher(voucher);

            if (!applyVoucherValidation.IsValid)
            {
                foreach (var error in applyVoucherValidation.Errors)
                    await _mediatorHandler.PublishNotification(new DomainNotification(error.ErrorCode, error.ErrorMessage));

                return false;
            }

            order.AddEvent(new VoucherApplyedOrderEvent(message.ClientId, order.Id, voucher.Id));
            order.AddEvent(new UpdatedOrderEvent(order.ClientId, order.Id, order.TotalValue));

            _orderRepository.UpdateOrder(order);
            return await _orderRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(IniciateOrderCommand message, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(message)) return false;

            var order = await _orderRepository.GetDraftOrderByClientId(message.ClientId);
            order.IniatializeOrder();

            var itensList = new List<Item>();
            order.OrderItems.ForEach(i => itensList.Add(new Item { Id = i.ProductId, Amount = i.Amount }));
            var orderProductList = new ProductsOrderList { OrderId = order.Id, Items = itensList };

            order.AddEvent
            (
              new IniciatedOrderEvent(order.Id, message.ClientId, order.TotalValue, orderProductList, message.CardName, message.CardNumber, message.CardExpirationDate, message.CardCvv)
            );

            _orderRepository.UpdateOrder(order);
            return await _orderRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(CancelOrderProcessReturnStockCommand message, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrderById(message.OrderId);

            if (order is null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("pedido", "Pedido não encontrado"));
                return false;
            }

            var itemList = new List<Item>();
            order.OrderItems.ForEach(i => itemList.Add(new Item { Id = i.Id, Amount = i.Amount }));
            var productOrderList = new ProductsOrderList { OrderId = order.Id, Items = itemList };

            order.AddEvent(new OrderProcessCancelledEvent(order.Id, order.ClientId, productOrderList));

            order.MakeItDraft();

            return await _orderRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(FinishOrderCommand message, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrderById(message.OrderId);

            if(order is null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("pedido", "Pedido não encontrado"));
                return false;
            }

            order.FinishOrder();

            order.AddEvent(new FinishedOrderEvent(message.OrderId));
            return await _orderRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(CancelOrderProcessCommand message, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrderById(message.OrderId);

            if (order is null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("pedido", "Pedido não encontrado"));
                return false;
            }

            order.MakeItDraft();
            return await _orderRepository.UnitOfWork.Commit();
        }

        private bool ValidateCommand(Command message)
        {
            if (message.IsValid()) return true;

            foreach (var error in message.ValidationResult.Errors)
            {
                _mediatorHandler.PublishNotification(new DomainNotification(message.MessageType, error.ErrorMessage));
            }

            return false;
        }
    }
}
