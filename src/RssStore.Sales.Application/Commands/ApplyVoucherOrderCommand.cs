using RssStore.Core.DomainObjects.Messages;
using RssStore.Sales.Application.Validations;
using System;

namespace RssStore.Sales.Application.Commands
{
    public class ApplyVoucherOrderCommand : Command
    {

        public Guid ClientId { get; private set; }
        public string VoucherCode { get; private set; }

        public ApplyVoucherOrderCommand(Guid clientId, string voucherCode)
        {
            ClientId = clientId;
            VoucherCode = voucherCode;
        }

        public override bool IsValid()
        {
            ValidationResult = new ApplyVoucherOrderValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
