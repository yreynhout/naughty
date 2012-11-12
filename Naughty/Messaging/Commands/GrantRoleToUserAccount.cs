using System;

namespace Seabites.Naughty.Messaging.Commands {
  public class GrantRoleToUserAccount {
    public readonly Guid UserAccountId;
    public readonly Guid RoleId;

    public GrantRoleToUserAccount(Guid userAccountId, Guid roleId) {
      UserAccountId = userAccountId;
      RoleId = roleId;
    }
  }
}