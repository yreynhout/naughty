using System;

namespace Seabites.Naughty.Messaging {
  public class AddedUserAccount {
    public readonly Guid UserAccountId;
    public readonly string Name;
    public AddedUserAccount(Guid userAccountId, string name) {
      UserAccountId = userAccountId;
      Name = name;
    }
  }
}