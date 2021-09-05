using RssStore.Core.DomainObjects.Messages;
using System;

namespace RssStore.Core.Data.EventSourcing
{
    public class StoredEvent : Event
    {
        public StoredEvent(Guid id, string type, DateTime ocurrencyDate, string data)
        {
            Id = id;
            Type = type;
            OcurrencyDate = ocurrencyDate;
            Data = data;
        }

        public Guid Id { get; set; }
        public string Type { get; set; }
        public DateTime OcurrencyDate { get; set; }
        public string Data { get; set; }

    }
}
