using System;

namespace Seabites.Naughty.Messaging.Commands {
  public class AddRoleGroup {
    public readonly Guid RoleGroupId;
    public readonly string Name;

    public AddRoleGroup(Guid roleGroupId, string name) {
      RoleGroupId = roleGroupId;
      Name = name;
    }
  }
}