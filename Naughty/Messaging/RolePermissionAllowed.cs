using System;

namespace Seabites.Naughty.Messaging {
  public class RolePermissionAllowed {
    public readonly Guid RoleId;
    public readonly Guid PermissionId;
    public RolePermissionAllowed(Guid roleId, Guid permissionId) {
      RoleId = roleId;
      PermissionId = permissionId;
    }
  }
}