using RssStore.Payment.AntiCorruption.Interfaces;
using RssStore.Payment.Business.DTOs;
using RssStore.Payment.Business.Entities;
using RssStore.Payment.Business.Enums;
using RssStore.Payment.Business.Interfaces;

namespace RssStore.Payment.AntiCorruption
{
    public class CreditCardPaymentFacade : ICreditCardPaymentFacade
    {
        private readonly IPayPalGateway _payPalGateway;
        private readonly IConfigurationManager _configurationManager;

        public CreditCardPaymentFacade(IPayPalGateway payPalGateway, IConfigurationManager configurationManager)
        {
            _payPalGateway = payPalGateway;
            _configurationManager = configurationManager;
        }

        public Transaction MakeOrderPayment(Order order, Business.Entities.Payment payment)
        {
            var apiKey = _configurationManager.GetValue("apiKey");
            var encriptionKey = _configurationManager.GetValue("encriptionKey");

            var serviceKey = _payPalGateway.GetPayPalServiceKey(apiKey, encriptionKey);
            var cardHashKey = _payPalGateway.GetCardHashKey(serviceKey, payment.CardNumber);

            var paymentResult = _payPalGateway.CommitTransaction(cardHashKey, order.Id.ToString(), payment.Value);

            var transaction = new Transaction(order.Id, payment.Id, order.Value);

            if (paymentResult)
            {
                transaction.ChangeTransactionStatus(TransactionStatus.Pago);
                return transaction;
            }

            transaction.ChangeTransactionStatus(TransactionStatus.Recusado);
            return transaction;
        }
    }
}
