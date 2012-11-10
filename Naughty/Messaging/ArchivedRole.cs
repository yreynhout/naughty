using System;

namespace Seabites.Naughty.Messaging {
  public class ArchivedRole {
    public readonly Guid RoleId;
    public ArchivedRole(Guid roleId) {
      RoleId = roleId;
    }
  }
}