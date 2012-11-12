using System;

namespace Seabites.Naughty.Messaging.Commands {
  public class RevokeRoleFromUserAccount {
    public readonly Guid UserAccountId;
    public readonly Guid RoleId;

    public RevokeRoleFromUserAccount(Guid userAccountId, Guid roleId) {
      UserAccountId = userAccountId;
      RoleId = roleId;
    }
  }
}