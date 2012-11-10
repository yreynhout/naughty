using System;

namespace Seabites.Naughty.Infrastructure {
  public class CompositeHandler : IHandle<object> {
    readonly IHandle<object>[] _handlers;

    public CompositeHandler(IHandle<object>[] handlers) {
      if (handlers == null) throw new ArgumentNullException("handlers");
      _handlers = handlers;
    }

    public void Handle(object message) {
      foreach (var handler in _handlers) {
        handler.Handle(message);
      }
    }
  }
}