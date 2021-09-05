using RssStore.Core.BaseEntity.DomainObjects;
using RssStore.Core.Interfaces.DomainObjects;
using RssStore.Register.Domain.ValueObjects;
using System;

namespace RssStore.Register.Domain.Entities
{
    public class Client : Entity, IAggregateRoot
    {
        public CompleteName CompleteName { get; private set; }
        public DateTime BirthDate { get; private set; }
        public IdentificationDocument IdentificationDocument { get; private set; }
        public string Email { get; private set; }
        public ContactNumber ContactNumber { get; private set; }
        public Address Address { get; private set; }

        public Client(CompleteName completeName, DateTime birthDate, IdentificationDocument identificationDocument, string email, ContactNumber contactNumber, Address address)
        {
            BirthDate = birthDate;
            IdentificationDocument = identificationDocument;
            Email = email;
            ContactNumber = contactNumber;
            Address = address;
        }
    }
}
