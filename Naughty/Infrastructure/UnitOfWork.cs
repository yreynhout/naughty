using System;
using System.Collections.Generic;
using System.Linq;

namespace Seabites.Naughty.Infrastructure {
  public class UnitOfWork : IUnitOfWork {
    readonly Dictionary<Guid, IAggregateRootEntity> _roots;

    public UnitOfWork() {
      _roots = new Dictionary<Guid, IAggregateRootEntity>();
    }

    public void Attach(IAggregateRootEntity root) {
      _roots.Add(root.Id, root);
    }

    public bool TryGet(Guid id, out IAggregateRootEntity root) {
      return _roots.TryGetValue(id, out root);
    }

    public IEnumerable<object> GetChanges() {
      return _roots.Values.Where(root => root.HasChanges()).SelectMany(root => root.GetChanges());
    }
  }
}