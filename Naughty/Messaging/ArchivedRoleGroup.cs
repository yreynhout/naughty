using System;

namespace Seabites.Naughty.Messaging {
  public class ArchivedRoleGroup {
    public readonly Guid RoleGroupId;

    public ArchivedRoleGroup(Guid roleGroupId) {
      RoleGroupId = roleGroupId;
    }
  }
}