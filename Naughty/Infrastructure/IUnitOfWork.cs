using System;

namespace Seabites.Naughty.Infrastructure {
  public interface IUnitOfWork {
    void Attach(IAggregateRootEntity root);
    bool TryGet(Guid id, out IAggregateRootEntity root);
  }
}