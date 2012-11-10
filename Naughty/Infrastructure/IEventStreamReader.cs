using System;

namespace Seabites.Naughty.Infrastructure {
  public interface IEventStreamReader {
    EventStream Read(Guid id);
  }
}