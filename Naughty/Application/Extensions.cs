using Seabites.Naughty.Infrastructure;

namespace Seabites.Naughty.Application {
  public static class Extensions {
    public static IHandle<SecurityContext<TMessage>> Secure<TMessage>(this IHandle<TMessage> handler,
                                                                      IMessageAuthorizer authorizer) {
      return new SecurityContextAwareHandler<TMessage>(authorizer, handler);
    }
  }
}