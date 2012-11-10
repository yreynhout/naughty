using Seabites.Naughty.Security;

namespace Seabites.Naughty {
  public class SecurityContext<TMessage> {
    readonly TMessage _message;
    readonly UserAccountId _userAccountId;

    public SecurityContext(TMessage message, UserAccountId userAccountId) {
      _message = message;
      _userAccountId = userAccountId;
    }

    public TMessage Message {
      get { return _message; }
    }

    public UserAccountId UserAccountId {
      get { return _userAccountId; }
    }
  }
}