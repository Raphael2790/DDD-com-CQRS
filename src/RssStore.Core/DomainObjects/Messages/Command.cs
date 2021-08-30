using FluentValidation.Results;
using MediatR;
using System;

namespace RssStore.Core.DomainObjects.Messages
{
    public abstract class Command : Message, IRequest<bool>
    {
        public DateTime TimeStamp { get; set; }
        public ValidationResult ValidationResult { get; set; }

        public Command()
        {
            TimeStamp = DateTime.Now;
        }

        public virtual bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
