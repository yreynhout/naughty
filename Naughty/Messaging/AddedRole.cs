using System;

namespace Seabites.Naughty.Messaging {
  public class AddedRole {
    public readonly Guid RoleId;
    public readonly string Name;
    public AddedRole(Guid roleId, string name) {
      RoleId = roleId;
      Name = name;
    }
  }
}