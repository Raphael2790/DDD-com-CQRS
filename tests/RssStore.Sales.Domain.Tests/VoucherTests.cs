using RssStore.Sales.Domain.Entities;
using RssStore.Sales.Domain.EntityValidations;
using RssStore.Sales.Domain.Enums;
using System;
using System.Linq;
using Xunit;

namespace RssStore.Sales.Domain.Tests
{
    public class VoucherTests
    {
        [Fact(DisplayName = "Validar voucher tipo valor válido")]
        [Trait("Categoria", "Vendas - Vocuher")]
        public void Voucher_ValidateVoucherValueType_ShouldBeValid()
        {
            //Arrange
            var voucher = new Voucher("PROMO-15", null, 15, 10, VoucherDiscountType.Value, DateTime.Now.AddDays(20), true, false);

            //Act
            var result = voucher.IsValidToApply();

            //Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Validar voucher tipo valor inválido")]
        [Trait("Categoria", "Vendas - Voucher")]
        public void Voucher_ValidateVocuherValueType_ShouldBeInvalid()
        {
            //Arrange
            var voucher = new Voucher("", null, null, 0, VoucherDiscountType.Value, DateTime.Now.AddDays(-1), false, true);

            //Act
            var result = voucher.IsValidToApply();

            //Assert
            Assert.False(result.IsValid);
            Assert.True(result.Errors.Count > 0);
            Assert.Contains(ApplyableVoucherValidation.CodeErrorMsg, result.Errors.Select(x => x.ErrorMessage));
            Assert.Contains(ApplyableVoucherValidation.ActiveErrorMsg, result.Errors.Select(x => x.ErrorMessage));
            Assert.Contains(ApplyableVoucherValidation.DiscountErrorMsg, result.Errors.Select(x => x.ErrorMessage));
            Assert.Contains(ApplyableVoucherValidation.ExpirationDateErrorMsg, result.Errors.Select(x => x.ErrorMessage));
            Assert.Contains(ApplyableVoucherValidation.ApplyedErrorMsg, result.Errors.Select(x => x.ErrorMessage));
            Assert.Contains(ApplyableVoucherValidation.AmountErrorMsg, result.Errors.Select(x => x.ErrorMessage));
        }

        [Fact(DisplayName = "Validar voucher tipo percentual válido")]
        [Trait("Categoria", "Vendas - Vocuher")]
        public void Voucher_ValidateVoucherPercentageType_ShouldBeValid()
        {
            //Arrange
            var voucher = new Voucher("PROMO-15", 15, null, 10, VoucherDiscountType.Percentage, DateTime.Now.AddDays(20), true, false);

            //Act
            var result = voucher.IsValidToApply();

            //Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Validar voucher tipo percentual inválido")]
        [Trait("Categoria", "Vendas - Vocuher")]
        public void Voucher_ValidateVoucherPercentageType_ShouldBeInvalid()
        {
            //Arrange
            var voucher = new Voucher("", null, null, 0, VoucherDiscountType.Percentage, DateTime.Now.AddDays(-1), false, true);

            //Act
            var result = voucher.IsValidToApply();

            //Assert
            Assert.False(result.IsValid);
            Assert.Contains(ApplyableVoucherValidation.CodeErrorMsg, result.Errors.Select(x => x.ErrorMessage));
            Assert.Contains(ApplyableVoucherValidation.ActiveErrorMsg, result.Errors.Select(x => x.ErrorMessage));
            Assert.Contains(ApplyableVoucherValidation.PercentualDiscountErrorMsg, result.Errors.Select(x => x.ErrorMessage));
            Assert.Contains(ApplyableVoucherValidation.ExpirationDateErrorMsg, result.Errors.Select(x => x.ErrorMessage));
            Assert.Contains(ApplyableVoucherValidation.ApplyedErrorMsg, result.Errors.Select(x => x.ErrorMessage));
            Assert.Contains(ApplyableVoucherValidation.AmountErrorMsg, result.Errors.Select(x => x.ErrorMessage));
        }
    }
}
