using RssStore.Core.BaseEntity.DomainObjects;
using RssStore.Core.DomainObjects.Validations;
using System;
using System.Collections.Generic;

namespace RssStore.Catalog.Domain.Entities
{
    public class Category : Entity
    {
        public Category(string name, int code)
        {
            Name = name;
            Code = code;

            Validate();
        }

        protected Category()
        {

        }

        public string Name { get; private set; }
        public int Code { get; private set; }

        // Entity Relations
        public ICollection<Product> Products { get; set; }

        public override string ToString()
        {
            return $"{Name} - {Code}";
        }
        private void Validate()
        {
            AssertionConcern.ValidateIsEmpty(Name, "O nome da categoria não pode ser vazio");
            AssertionConcern.ValidateIsEqual(Code, 0, "O código da categoria não pode ser 0");
        }
    }
}
