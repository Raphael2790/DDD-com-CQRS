using RssStore.Core.DomainObjects;
using RssStore.Core.DomainObjects.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace RssStore.Catalog.Domain.Tests
{
    public class DimensionsUnitTest
    {
        [Fact]
        public void Dimensions_Validate_ValidationShouldReturnExceptions()
        {
            //Assert & Act $ Assert
            var ex = Assert.Throws<DomainException>(() => new Dimensions(0, 1, 1));

            Assert.Equal("O campo largura não pode ser menor que 1cm", ex.Message);

            ex = Assert.Throws<DomainException>(() => new Dimensions(1, 0, 1));

            Assert.Equal("O campo altura não pode ser menor que 1 cm", ex.Message);

            ex = Assert.Throws<DomainException>(() => new Dimensions(1, 1, 0));

            Assert.Equal("O campo profundidade não pode ser menor que 1 cm", ex.Message);
        }
    }
}
