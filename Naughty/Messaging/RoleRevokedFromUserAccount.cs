using System;

namespace Seabites.Naughty.Messaging {
  public class RoleRevokedFromUserAccount {
    public readonly Guid UserAccountId;
    public readonly Guid RoleId;
    public RoleRevokedFromUserAccount(Guid userAccountId, Guid roleId) {
      UserAccountId = userAccountId;
      RoleId = roleId;
    }
  }
}