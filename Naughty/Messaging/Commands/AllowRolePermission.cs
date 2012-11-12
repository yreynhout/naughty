using System;

namespace Seabites.Naughty.Messaging.Commands {
  public class AllowRolePermission {
    public readonly Guid RoleId;
    public readonly Guid PermissionId;

    public AllowRolePermission(Guid roleId, Guid permissionId) {
      RoleId = roleId;
      PermissionId = permissionId;
    }
  }
}