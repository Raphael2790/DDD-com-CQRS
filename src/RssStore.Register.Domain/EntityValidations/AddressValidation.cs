using FluentValidation;
using RssStore.Register.Domain.Entities;

namespace RssStore.Register.Domain.EntityValidations
{
    public class AddressValidation : AbstractValidator<Address>
    {
        public AddressValidation()
        {
            RuleFor(x => x.Street)
                .NotEmpty()
                .WithMessage("O logradouro deve ser informado");

            RuleFor(x => x.Number)
                .NotEmpty()
                .WithMessage("O número do logradouro deve ser informado");

            RuleFor(x => x.Neighborhood)
                .NotEmpty()
                .WithMessage("O bairro deve ser informado");

            RuleFor(x => x.Country)
                .NotEmpty()
                .WithMessage("O pais deve ser informado");

            RuleFor(x => x.City)
                .NotEmpty()
                .WithMessage("A cidade deve ser informada");

            RuleFor(x => x.ZipCode)
                .NotEmpty()
                .WithMessage("O CEP deve ser informado");

            RuleFor(x => x.AddressType)
                .IsInEnum()
                .WithMessage("Selecione o tipo de endereço");
        }
    }
}
