using MediatR;
using RssStore.Core.DomainObjects.DTOs;
using RssStore.Core.DomainObjects.Messages.CommonMessages.IntegrationEvents;
using RssStore.Payment.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RssStore.Payment.Business.EventHandlers
{
    public class PaymentEventHandler : INotificationHandler<ConfirmedOrderStockEvent>
    {
        private readonly IPaymentService _paymentService;
        public PaymentEventHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }


        public async Task Handle(ConfirmedOrderStockEvent message, CancellationToken cancellationToken)
        {
            var orderPayment = new OrderPayment
            {
                OrderId = message.OrderId,
                ClientId = message.ClientId,
                Total = message.Total,
                CardCvv = message.CardCvv,
                CardName = message.CardName,
                CardNumber = message.CardNumber,
                CardExpirationDate = message.CardExpirationDate
            };

            await _paymentService.MakeOrderPayment(orderPayment);
        }
    }
}
