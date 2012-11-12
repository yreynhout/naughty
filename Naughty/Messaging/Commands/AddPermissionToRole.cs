using System;

namespace Seabites.Naughty.Messaging.Commands {
  public class AddPermissionToRole {
    public readonly Guid RoleId;
    public readonly Guid PermissionId;

    public AddPermissionToRole(Guid roleId, Guid permissionId) {
      RoleId = roleId;
      PermissionId = permissionId;
    }
  }
}