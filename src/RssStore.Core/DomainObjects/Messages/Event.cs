using MediatR;
using System;

namespace RssStore.Core.DomainObjects.Messages
{
    public abstract class Event : Message, INotification
    {
        public DateTime TimeStamp { get; set; }
        protected Event()
        {
            TimeStamp = DateTime.Now;
        }
    }
}
