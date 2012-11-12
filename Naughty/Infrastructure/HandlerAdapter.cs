using System;

namespace Seabites.Naughty.Infrastructure {
  public class HandlerAdapter<TMessage> : IHandle<object> {
    readonly IHandle<TMessage> _handler;

    public HandlerAdapter(IHandle<TMessage> handler) {
      if (handler == null) throw new ArgumentNullException("handler");
      _handler = handler;
    }

    public void Handle(object message) {
      if (message is TMessage) _handler.Handle((TMessage) message);
    }
  }
}