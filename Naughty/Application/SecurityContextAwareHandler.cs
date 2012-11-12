using System;
using Seabites.Naughty.Infrastructure;

namespace Seabites.Naughty.Application {
  public class SecurityContextAwareHandler<TMessage> : IHandle<SecurityContext<TMessage>> {
    readonly IMessageAuthorizer _authorizer;
    readonly IHandle<TMessage> _handler;

    public SecurityContextAwareHandler(
      IMessageAuthorizer authorizer,
      IHandle<TMessage> handler) {
      if (authorizer == null) throw new ArgumentNullException("authorizer");
      if (handler == null) throw new ArgumentNullException("handler");
      _authorizer = authorizer;
      _handler = handler;
    }

    public void Handle(SecurityContext<TMessage> context) {
      _authorizer.Authorize(context.Message, context.Request);
      _handler.Handle(context.Request);
    }
  }
}