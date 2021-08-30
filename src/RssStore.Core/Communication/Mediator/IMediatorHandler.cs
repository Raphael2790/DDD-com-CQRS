using RssStore.Core.DomainObjects.Messages;
using RssStore.Core.DomainObjects.Messages.CommonMessages.Notifications;
using System.Threading.Tasks;

namespace RssStore.Core.Communication.Mediator
{
    public interface IMediatorHandler
    {
        Task PublishEvent<T>(T evento) where T : Event;
        Task<bool> SendCommand<T>(T command) where T : Command;
        Task PublishNotification<T>(T notification) where T : DomainNotification;
    }
}
