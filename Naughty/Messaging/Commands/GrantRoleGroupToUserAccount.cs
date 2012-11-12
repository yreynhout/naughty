using System;

namespace Seabites.Naughty.Messaging.Commands {
  public class GrantRoleGroupToUserAccount {
    public readonly Guid UserAccountId;
    public readonly Guid RoleGroupId;

    public GrantRoleGroupToUserAccount(Guid userAccountId, Guid roleGroupId) {
      UserAccountId = userAccountId;
      RoleGroupId = roleGroupId;
    }
  }
}