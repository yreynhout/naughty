using System;
using System.Collections.Generic;

namespace Seabites.Naughty.Infrastructure {
  public class EventStreamReader : IEventStreamReader {
    readonly Dictionary<Guid, List<object>> _storage;

    public EventStreamReader(Dictionary<Guid, List<object>> storage) {
      if (storage == null) throw new ArgumentNullException("storage");
      _storage = storage;
    }

    public EventStream Read(Guid id) {
      List<object> events;
      if(_storage.TryGetValue(id, out events)) {
        return new EventStream(id, events.ToArray());
      }
      return new EventStream(id, new object[0]);
    }
  }
}