using RssStore.Core.BaseEntity.DomainObjects;
using RssStore.Core.DomainObjects;
using RssStore.Register.Domain.EntityValidations;
using RssStore.Register.Domain.Enums;

namespace RssStore.Register.Domain.Entities
{
    public class Address : Entity
    {

        public string Street { get; private set; }
        public string Number { get; private set; }
        public string Neighborhood { get; private set; }
        public string ZipCode { get; set; }
        public string City { get; private set; }
        public string Country { get; private set; }
        public AddressType AddressType { get; private set; }

        public Address(string street, string number, string neighborhood, string zipCode, string city, string country, AddressType addressType)
        {
            Street = street;
            Number = number;
            Neighborhood = neighborhood;
            ZipCode = zipCode;
            City = city;
            Country = country;
            AddressType = addressType;
        }

        protected Address() { }

        public override bool IsValid()
        {
            var validationResult = new AddressValidation().Validate(this);

            foreach (var item in validationResult.Errors)
                throw new DomainException(item.ErrorMessage);

            return validationResult.IsValid;
        }
    }
}
