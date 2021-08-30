using MediatR;
using RssStore.Catalog.Domain.Events;
using RssStore.Catalog.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace RssStore.Catalog.Domain.EventHandlers
{
    public class ProductStockEventHandler : INotificationHandler<LowProductStockEvent>
    {
        private readonly IProductRepository _productRepository;

        public ProductStockEventHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task Handle(LowProductStockEvent message, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetById(message.AggregateId);

            //Uma logica para processar o evento
        }
    }
}
