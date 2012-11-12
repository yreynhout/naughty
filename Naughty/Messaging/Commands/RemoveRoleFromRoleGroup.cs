using System;

namespace Seabites.Naughty.Messaging.Commands {
  public class RemoveRoleFromRoleGroup {
    public readonly Guid RoleGroupId;
    public readonly Guid RoleId;

    public RemoveRoleFromRoleGroup(Guid roleGroupId, Guid roleId) {
      RoleGroupId = roleGroupId;
      RoleId = roleId;
    }
  }
}