using RssStore.Payment.Business.DTOs;
using RssStore.Payment.Business.Entities;

namespace RssStore.Payment.Business.Interfaces
{
    public interface ICreditCardPaymentFacade
    {
        Transaction MakeOrderPayment(Order order, Entities.Payment payment);
    }
}
