using System;

namespace Seabites.Naughty.Messaging {
  public class RoleGrantedToUserAccount {
    public readonly Guid UserAccountId;
    public readonly Guid RoleId;
    public RoleGrantedToUserAccount(Guid userAccountId, Guid roleId) {
      UserAccountId = userAccountId;
      RoleId = roleId;
    }
  }
}