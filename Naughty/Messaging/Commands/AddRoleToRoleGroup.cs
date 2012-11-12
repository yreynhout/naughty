using System;

namespace Seabites.Naughty.Messaging.Commands {
  public class AddRoleToRoleGroup {
    public readonly Guid RoleGroupId;
    public readonly Guid RoleId;

    public AddRoleToRoleGroup(Guid roleGroupId, Guid roleId) {
      RoleGroupId = roleGroupId;
      RoleId = roleId;
    }
  }
}