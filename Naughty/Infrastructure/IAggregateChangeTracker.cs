using System.Collections.Generic;

namespace Seabites.Naughty.Infrastructure {
  public interface IAggregateChangeTracker {
    bool HasChanges();
    IEnumerable<object> GetChanges();
    void ClearChanges();
  }
}