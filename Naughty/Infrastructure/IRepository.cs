using System;

namespace Seabites.Naughty.Infrastructure {
  public interface IRepository<TAggregateRootEntity> where TAggregateRootEntity : IAggregateRootEntity {
    TAggregateRootEntity Get(Guid id);
    void Add(TAggregateRootEntity root);
  }
}