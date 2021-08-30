using RssStore.Core.Interfaces.DomainObjects;
using System;

namespace RssStore.Core.Data
{
    public interface IRepository<T> : IDisposable where T : IAggregateRoot 
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
