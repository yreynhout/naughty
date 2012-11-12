using System;

namespace Seabites.Naughty.Messaging.Events {
  public class ArchivedRole {
    public readonly Guid RoleId;

    public ArchivedRole(Guid roleId) {
      RoleId = roleId;
    }
  }
}