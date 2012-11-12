using System;

namespace Seabites.Naughty.Messaging.Commands {
  public class DisableUserAccount {
    public readonly Guid UserAccountId;

    public DisableUserAccount(Guid userAccountId) {
      UserAccountId = userAccountId;
    }
  }
}