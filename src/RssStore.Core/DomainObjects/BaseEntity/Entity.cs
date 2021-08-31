using RssStore.Core.DomainObjects.Messages;
using System;
using System.Collections.Generic;

namespace RssStore.Core.BaseEntity.DomainObjects
{
    public abstract class Entity
    {
        private List<Event> _notificationEvents;

        public IReadOnlyCollection<Event> NoticationEvents => _notificationEvents?.AsReadOnly();

        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public void AddEvents(Event eventItem)
        {
            _notificationEvents = _notificationEvents ?? new List<Event>();
            _notificationEvents.Add(eventItem);
        }

        public void RemoveEvent(Event eventItem)
        {
            _notificationEvents?.Remove(eventItem);
        }

        public void ClearEvents()
        {
            _notificationEvents?.Clear();
        }

        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity;

            if(ReferenceEquals(this, compareTo)) return true;
            if(ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);
        }

        //Sobrescreve o operador de igualdade entre entidades
        public static bool operator ==(Entity a,Entity b)
        {
            if (ReferenceEquals(null, a) && ReferenceEquals(null, b))
                return true;

            if (ReferenceEquals(null, a) || ReferenceEquals(null, b))
                return false;

            return a.Equals(b);
        }

        //Sobrescreve o operador de diferenção entre entidades
        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        //Ajuda na distinção da comparação de igualdade entre os objetos
        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{GetType().Name} [Id={Id}]";
        }

        public virtual bool IsValid() 
        {
            throw new NotImplementedException();
        }
    }
}
