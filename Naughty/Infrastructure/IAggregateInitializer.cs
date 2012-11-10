using System.Collections.Generic;

namespace Seabites.Naughty.Infrastructure {
  public interface IAggregateInitializer {
    void Initialize(IEnumerable<object> events);
  }
}