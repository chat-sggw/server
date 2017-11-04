using System;
using Neat.CQRSLite.Contract.Domain;

namespace ChatSggw.Domain
{
    public interface IRepository<TAggregate, TKey> where TAggregate : IAggregateRoot<TKey>
    {
        TAggregate Get(TKey id);
        void Save(TAggregate entity);
        void Delete(TAggregate entity);
    }

    public interface IRepository<TAggregate> : IRepository<TAggregate, Guid> 
        where TAggregate : IAggregateRoot
    {
    }
}