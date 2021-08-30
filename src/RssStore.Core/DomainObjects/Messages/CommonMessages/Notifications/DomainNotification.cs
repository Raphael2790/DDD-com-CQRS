using MediatR;
using System;

namespace RssStore.Core.DomainObjects.Messages.CommonMessages.Notifications
{
    public class DomainNotification : Message, INotification
    {
        public DomainNotification(string key, string value)
        {
            Key = key;
            Value = value;
            TimeStamp = DateTime.Now;
            Version = 1;
            DomainNoticiationId = Guid.NewGuid();
        }

        public DateTime TimeStamp { get; private set; }
        public Guid DomainNoticiationId { get; private set; }
        public string Key { get; private set; }
        public string Value { get; private set; }
        public int Version { get; private set; }

    }
}
