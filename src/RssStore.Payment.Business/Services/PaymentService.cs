using RssStore.Core.Communication.Mediator;
using RssStore.Core.DomainObjects.DTOs;
using RssStore.Core.DomainObjects.Messages.CommonMessages.IntegrationEvents;
using RssStore.Core.DomainObjects.Messages.CommonMessages.Notifications;
using RssStore.Payment.Business.DTOs;
using RssStore.Payment.Business.Entities;
using RssStore.Payment.Business.Enums;
using RssStore.Payment.Business.Interfaces;
using System.Threading.Tasks;
using PaymentOrder = RssStore.Payment.Business.Entities.Payment;

namespace RssStore.Payment.Business.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ICreditCardPaymentFacade _creditCardPaymentFacade;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public PaymentService(ICreditCardPaymentFacade creditCardPaymentFacade, IPaymentRepository paymentRepository, IMediatorHandler mediatorHandler)
        {
            _creditCardPaymentFacade = creditCardPaymentFacade;
            _paymentRepository = paymentRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<Transaction> MakeOrderPayment(OrderPayment orderPayment)
        {
            var order = new Order
            {
                Id = orderPayment.OrderId,
                Value = orderPayment.Total
            };

            var payment = new PaymentOrder(orderPayment.OrderId, orderPayment.Total, orderPayment.CardName, orderPayment.CardNumber, orderPayment.CardExpirationDate, orderPayment.CardCvv);

            var transaction = _creditCardPaymentFacade.MakeOrderPayment(order, payment);

            if (transaction.TransactionStatus == TransactionStatus.Pago)
            {
                payment.AddEvent(new AuthorizedPaymentEvent(order.Id, orderPayment.ClientId, transaction.PaymentId, transaction.Id, order.Value));

                _paymentRepository.Add(payment);
                _paymentRepository.AddTransaction(transaction);

                await _paymentRepository.UnitOfWork.Commit();
                return transaction;
            }

            await _mediatorHandler.PublishNotification(new DomainNotification("pagamento", "A operadora do cartão não autorizou o pagamento"));
            await _mediatorHandler.PublishEvent(new UnauthorizedPaymentEvent(order.Id, orderPayment.ClientId, transaction.PaymentId, transaction.Id, order.Value));

            return transaction;
        }
    }
}
