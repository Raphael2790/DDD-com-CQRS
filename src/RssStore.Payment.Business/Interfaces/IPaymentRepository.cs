using RssStore.Core.Data;
using RssStore.Payment.Business.Entities;

namespace RssStore.Payment.Business.Interfaces
{
    public interface IPaymentRepository : IRepository<Entities.Payment>
    {

        void AddTransaction(Transaction transaction);
        void Add(Entities.Payment payment);
    }
}
