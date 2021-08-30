using MediatR;
using RssStore.Core.DomainObjects.Messages;
using RssStore.Core.DomainObjects.Messages.CommonMessages.Notifications;
using System.Threading.Tasks;

namespace RssStore.Core.Communication.Mediator
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediatR;
        public MediatorHandler(IMediator mediatoR)
        {
            _mediatR = mediatoR;
        }
        public async Task PublishEvent<T>(T evento) where T : Event
        {
            await _mediatR.Publish(evento);
        }

        public async Task PublishNotification<T>(T notification) where T : DomainNotification
        {
            await _mediatR.Publish(notification);
        }

        public async Task<bool> SendCommand<T>(T command) where T : Command
        {
            return await _mediatR.Send(command);
        }
    }
}
