using System;

namespace Seabites.Naughty.Messaging {
  public class RoleGroupGrantedToUserAccount {
    public readonly Guid UserAccountId;
    public readonly Guid RoleGroupId;
    public RoleGroupGrantedToUserAccount(Guid userAccountId, Guid roleGroupId) {
      UserAccountId = userAccountId;
      RoleGroupId = roleGroupId;
    }
  }
}