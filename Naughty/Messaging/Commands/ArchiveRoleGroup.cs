using System;

namespace Seabites.Naughty.Messaging.Commands {
  public class ArchiveRoleGroup {
    public readonly Guid RoleGroupId;

    public ArchiveRoleGroup(Guid roleGroupId) {
      RoleGroupId = roleGroupId;
    }
  }
}