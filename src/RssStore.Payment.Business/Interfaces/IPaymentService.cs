using RssStore.Core.DomainObjects.DTOs;
using RssStore.Payment.Business.Entities;
using System.Threading.Tasks;

namespace RssStore.Payment.Business.Interfaces
{
    public interface IPaymentService
    {
        Task<Transaction> MakeOrderPayment(OrderPayment orderPayment);
    }
}
