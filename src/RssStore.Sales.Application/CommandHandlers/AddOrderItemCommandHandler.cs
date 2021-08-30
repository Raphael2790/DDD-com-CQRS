using MediatR;
using RssStore.Core.Communication.Mediator;
using RssStore.Core.DomainObjects.Messages;
using RssStore.Core.DomainObjects.Messages.CommonMessages.Notifications;
using RssStore.Sales.Application.Commands;
using RssStore.Sales.Domain.Entities;
using RssStore.Sales.Domain.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RssStore.Sales.Application.CommandHandlers
{
    public class AddOrderItemCommandHandler : IRequestHandler<AddOrderItemCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMediatorHandler _mediatorHandler;
        public AddOrderItemCommandHandler(IOrderRepository orderRepository, IMediatorHandler mediatorHandler)
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
            }

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
