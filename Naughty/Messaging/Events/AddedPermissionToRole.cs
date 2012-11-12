using System;

namespace Seabites.Naughty.Messaging.Events {
  public class AddedPermissionToRole {
    public readonly Guid RoleId;
    public readonly Guid PermissionId;

    public AddedPermissionToRole(Guid roleId, Guid permissionId) {
      RoleId = roleId;
      PermissionId = permissionId;
    }
  }
}