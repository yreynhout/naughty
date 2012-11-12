using System;

namespace Seabites.Naughty.Messaging.Commands {
  public class AddRole {
    public readonly Guid RoleId;
    public readonly string Name;

    public AddRole(Guid roleId, string name) {
      RoleId = roleId;
      Name = name;
    }
  }
}