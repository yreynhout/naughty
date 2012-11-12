using System;

namespace Seabites.Naughty.Messaging.Commands {
  public class RevokeRoleGroupFromUserAccount {
    public readonly Guid UserAccountId;
    public readonly Guid RoleGroupId;

    public RevokeRoleGroupFromUserAccount(Guid userAccountId, Guid roleGroupId) {
      UserAccountId = userAccountId;
      RoleGroupId = roleGroupId;
    }
  }
}