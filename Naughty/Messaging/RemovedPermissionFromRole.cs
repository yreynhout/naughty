using System;

namespace Seabites.Naughty.Messaging {
  public class RemovedPermissionFromRole {
    public readonly Guid RoleId;
    public readonly Guid PermissionId;
    public RemovedPermissionFromRole(Guid roleId, Guid permissionId) {
      RoleId = roleId;
      PermissionId = permissionId;
    }
  }
}