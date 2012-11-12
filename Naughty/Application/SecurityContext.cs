using Seabites.Naughty.Security;

namespace Seabites.Naughty.Application {
  public class SecurityContext<TMessage> {
    readonly TMessage _request;
    readonly UserAccountId _requester;

    public SecurityContext(UserAccountId requester, TMessage request) {
      _request = request;
      _requester = requester;
    }

    public TMessage Request {
      get { return _request; }
    }

    public UserAccountId Message {
      get { return _requester; }
    }
  }
}