using System;

namespace Seabites.Naughty.Messaging.Commands {
  public class RemovePermissionFromRole {
    public readonly Guid RoleId;
    public readonly Guid PermissionId;

    public RemovePermissionFromRole(Guid roleId, Guid permissionId) {
      RoleId = roleId;
      PermissionId = permissionId;
    }
  }
}