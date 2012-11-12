using System;

namespace Seabites.Naughty.Messaging.Commands {
  public class ArchiveRole {
    public readonly Guid RoleId;

    public ArchiveRole(Guid roleId) {
      RoleId = roleId;
    }
  }
}