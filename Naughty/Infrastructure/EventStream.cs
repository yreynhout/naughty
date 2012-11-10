using System;

namespace Seabites.Naughty.Infrastructure {
  public class EventStream {
    public readonly Guid Id;
    public readonly object[] Events;
    public EventStream(Guid id, object[] events) {
      Id = id;
      Events = events;
    }
  }
}