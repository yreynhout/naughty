using System;

namespace Seabites.Naughty.Messaging.Events {
  public class DisabledUserAccount {
    public readonly Guid UserAccountId;

    public DisabledUserAccount(Guid userAccountId) {
      UserAccountId = userAccountId;
    }
  }
}