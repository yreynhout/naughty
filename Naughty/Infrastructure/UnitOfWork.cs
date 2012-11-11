using System;
using System.Collections.Generic;
using System.Linq;

namespace Seabites.Naughty.Infrastructure {
  public class UnitOfWork : IUnitOfWork {
    readonly List<IAggregateRootEntity> _roots;

    public UnitOfWork() {
      _roots = new List< IAggregateRootEntity>();
    }

    public void Attach(IAggregateRootEntity root) {
      _roots.Add(root);
    }

    public bool TryGet(Guid id, out IAggregateRootEntity root) {
      root = _roots.Find(item => item.Id == id);
      return root != null;
    }

    public IEnumerable<object> GetChanges() {
      return _roots.Where(root => root.HasChanges()).SelectMany(root => root.GetChanges());
    }
  }
}