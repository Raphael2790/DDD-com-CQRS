using MediatR;
using RssStore.Core.Communication.Mediator;
using RssStore.Core.DomainObjects.Messages;
using RssStore.Core.DomainObjects.Messages.CommonMessages.Notifications;
using RssStore.Sales.Application.Commands;
using RssStore.Sales.Application.Events;
using RssStore.Sales.Domain.Entities;
using RssStore.Sales.Domain.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RssStore.Sales.Application.CommandHandlers
{
    //Todos comandos lançados referentes a uma entidade podem ser agrupados ou separados 
    public class OrderItemCommandHandler : 
        IRequestHandler<AddOrderItemCommand, bool>,
        IRequestHandler<ApplyVoucherOrderCommand, bool>,
        IRequestHandler<RemoveOrderItemCommand, bool>,
        IRequestHandler<UpdateOrderItemCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMediatorHandler _mediatorHandler;
        public OrderItemCommandHandler(IOrderRepository orderRepository, IMediatorHandler mediatorHandler)
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
