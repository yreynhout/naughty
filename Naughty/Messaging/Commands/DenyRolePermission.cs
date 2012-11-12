using System;

namespace Seabites.Naughty.Messaging.Commands {
  public class DenyRolePermission {
    public readonly Guid RoleId;
    public readonly Guid PermissionId;

    public DenyRolePermission(Guid roleId, Guid permissionId) {
      RoleId = roleId;
      PermissionId = permissionId;
    }
  }
}