using Seabites.Naughty.Security;

namespace Seabites.Naughty.Application {
  public class SecurityContext<TMessage> {
    readonly TMessage _message;
    readonly UserAccountId _accountId;

    public SecurityContext(UserAccountId accountId, TMessage message) {
      _message = message;
      _accountId = accountId;
    }

    public TMessage Message {
      get { return _message; }
    }

    public UserAccountId AccountId {
      get { return _accountId; }
    }
  }
}