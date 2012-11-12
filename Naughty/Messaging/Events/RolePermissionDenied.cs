using System;

namespace Seabites.Naughty.Messaging.Events {
  public class RolePermissionDenied {
    public readonly Guid RoleId;
    public readonly Guid PermissionId;

    public RolePermissionDenied(Guid roleId, Guid permissionId) {
      RoleId = roleId;
      PermissionId = permissionId;
    }
  }
}