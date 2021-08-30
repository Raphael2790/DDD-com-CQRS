using RssStore.Catalog.Domain.Entities;
using RssStore.Core.DomainObjects;
using RssStore.Core.DomainObjects.ValueObjects;
using System;
using Xunit;

namespace RssStore.Catalog.Domain.Tests
{
    public class ProductUnitTests
    {
        [Fact]
        public void Product_Validate_ValidationsShouldReturnExceptions()
        {
            //Assert & Act & Assert

            var ex = Assert.Throws<DomainException>(() => new Product(string.Empty, "Descri��o", false, 10M, DateTime.Now, "Imagem", Guid.NewGuid(),new Dimensions(1, 1, 1)));

            Assert.Equal("O campo Nome n�o pode estar vazio", ex.Message);

            ex = Assert.Throws<DomainException>(() => new Product("Nome", string.Empty, false, 10M, DateTime.Now, "Imagem", Guid.NewGuid(), new Dimensions(1, 1, 1)));

            Assert.Equal("O campo descri��o do produto n�o pode estar vazio", ex.Message);

            ex = Assert.Throws<DomainException>(() => new Product("Nome", "Descri��o", false, 10M, DateTime.Now, "Imagem", Guid.Empty, new Dimensions(1, 1, 1)));

            Assert.Equal("O campo CategoriaId do produto deve ser informado", ex.Message);

            ex = Assert.Throws<DomainException>(() => new Product("Nome", "Descri��o", false, 0, DateTime.Now, "Imagem", Guid.NewGuid(), new Dimensions(1, 1, 1)));

            Assert.Equal("O campo valor do produto n�o pode ser menor ou igual a 0", ex.Message);

            ex = Assert.Throws<DomainException>(() => new Product("Nome", "Descri��o", false, 10M, DateTime.Now, string.Empty, Guid.NewGuid(), new Dimensions(1, 1, 1)));

            Assert.Equal("O campo imagem do produto n�o pode estar vazio", ex.Message);
        }
    }
}
