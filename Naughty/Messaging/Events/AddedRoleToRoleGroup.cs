using System;

namespace Seabites.Naughty.Messaging.Events {
  public class AddedRoleToRoleGroup {
    public readonly Guid RoleGroupId;
    public readonly Guid RoleId;

    public AddedRoleToRoleGroup(Guid roleGroupId, Guid roleId) {
      RoleGroupId = roleGroupId;
      RoleId = roleId;
    }
  }
}