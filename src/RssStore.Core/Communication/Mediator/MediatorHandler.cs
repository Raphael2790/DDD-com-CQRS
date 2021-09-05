using MediatR;
using RssStore.Core.Communication.DomainEvents;
using RssStore.Core.Data.EventSourcing.Interfaces;
using RssStore.Core.DomainObjects.Messages;
using RssStore.Core.DomainObjects.Messages.CommonMessages.Notifications;
using System.Threading.Tasks;

namespace RssStore.Core.Communication.Mediator
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediatR;
        private readonly IEventSourcingRepository _eventSourcingRepository;
        public MediatorHandler(IMediator mediatoR, IEventSourcingRepository eventSourcingRepository)
        {
            _mediatR = mediatoR;
            _eventSourcingRepository = eventSourcingRepository;
        }

        public async Task PublishEvent<T>(T evento) where T : Event
        {
            await _mediatR.Publish(evento);

            if (!(evento.GetType().BaseType?.Name == "DomainEvent"))
                await _eventSourcingRepository.SaveEvent(evento);
        }

        public async Task PublishNotification<T>(T notification) where T : DomainNotification
        {
            await _mediatR.Publish(notification);
        }

        public async Task<bool> SendCommand<T>(T command) where T : Command
        {
            return await _mediatR.Send(command);
        }

        public async Task PublishDomainEvent<T>(T evento) where T : DomainEvent
        {
            await _mediatR.Publish(evento);
        }
    }
}
