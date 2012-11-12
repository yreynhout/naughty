using System;

namespace Seabites.Naughty.Messaging.Events {
  public class RoleGroupRevokedFromUserAccount {
    public readonly Guid UserAccountId;
    public readonly Guid RoleGroupId;

    public RoleGroupRevokedFromUserAccount(Guid userAccountId, Guid roleGroupId) {
      UserAccountId = userAccountId;
      RoleGroupId = roleGroupId;
    }
  }
}