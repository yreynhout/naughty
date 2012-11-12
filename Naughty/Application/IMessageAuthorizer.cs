using Seabites.Naughty.Security;

namespace Seabites.Naughty.Application {
  public interface IMessageAuthorizer {
    void Authorize(UserAccountId account, object message);
  }
}