using RssStore.Core.Data;
using RssStore.Payment.Business.Entities;
using RssStore.Payment.Business.Interfaces;
using RssStore.Payment.Data.Context;

namespace RssStore.Payment.Data.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentContext _context;
        public PaymentRepository(PaymentContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Add(Business.Entities.Payment payment)
        {
            _context.Payments.Add(payment);
        }

        public void AddTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
