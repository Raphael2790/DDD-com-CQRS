using RssStore.Core.BaseEntity.DomainObjects;
using RssStore.Core.DomainObjects;
using RssStore.Core.DomainObjects.Validations;
using RssStore.Core.DomainObjects.ValueObjects;
using RssStore.Core.Interfaces.DomainObjects;
using System;

namespace RssStore.Catalog.Domain.Entities
{
    public class Product : Entity, IAggregateRoot
    {
        public Product(string name, string description, bool active, decimal value, DateTime registerDate, string image, Guid categoryId, Dimensions dimensions)
        {
            Name = name;
            Description = description;
            Active = active;
            Value = value;
            RegisterDate = registerDate;
            Image = image;
            CategoryId = categoryId;
            Dimensions = dimensions;

            Validate();
        }

        protected Product() { }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool Active { get; private set; }
        public decimal Value { get; private set; }
        public DateTime RegisterDate { get; private set; }
        public string Image { get; private set; }
        public int StockAmount { get; private set; }
        public Guid CategoryId { get; set; }
        public Dimensions Dimensions { get; private set; } 

        public Category Category { get; private set; }

        //metodos add hook que servem para modificar estados da entidade
        public void Activate() => Active = true;
        public void DeActivate() => Active = false;
        public void ChangeCategory(Category category)
        {
            Category = category;
            CategoryId = category.Id;
        }

        public void ChangeDescription(string description)
        {
            AssertionConcern.ValidateIsEmpty(description, "A nova descrição do produto não pode ser vazia");
            Description = description;
        }

        public void DebitStockAmount(int amount)
        {
            //tranformar a quantidade em positiva caso seja passo um valor negativo
            if (amount < 0) amount *= -1;
            if (!HasStockAmount(amount)) throw new DomainException("Estoque Insuficiente!");
            StockAmount -= amount;
        }

        public void FillStockAmount(int amount)
        {
            StockAmount += amount;
        }

        public bool HasStockAmount(int amount)
        {
            return StockAmount >= amount;
        }

        private void Validate()
        {
            AssertionConcern.ValidateIsEmpty(Name, "O campo Nome não pode estar vazio");
            AssertionConcern.ValidateIsEmpty(Description, "O campo descrição do produto não pode estar vazio");
            AssertionConcern.ValidateIsEqual(CategoryId, Guid.Empty, "O campo CategoriaId do produto deve ser informado");
            AssertionConcern.ValidateIsLessThen(Value, 1, "O campo valor do produto não pode ser menor ou igual a 0");
            AssertionConcern.ValidateIsEmpty(Image, "O campo imagem do produto não pode estar vazio");
        }
    }
}
