using System;

namespace Seabites.Naughty.Messaging.Commands {
  public class AddUserAccount {
    public readonly Guid UserAccountId;
    public readonly string Name;

    public AddUserAccount(Guid userAccountId, string name) {
      UserAccountId = userAccountId;
      Name = name;
    }
  }
}