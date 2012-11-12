using System;
using System.Collections.Generic;
using System.Linq;
using Seabites.Naughty.Infrastructure;
using Seabites.Naughty.Messaging.Events;

namespace Seabites.Naughty.Projections {
  public class MemoryRoleGroupLookup :
    ILookupRolesOfRoleGroup,
    IHandle<AddedRoleToRoleGroup>,
    IHandle<RemovedRoleFromRoleGroup> {
    readonly Dictionary<Guid, HashSet<Guid>> _rolesInRoleGroup;

    public MemoryRoleGroupLookup() {
      _rolesInRoleGroup = new Dictionary<Guid, HashSet<Guid>>();
    }

    public void Handle(AddedRoleToRoleGroup message) {
      HashSet<Guid> roles;
      if (!_rolesInRoleGroup.TryGetValue(message.RoleGroupId, out roles)) {
        roles = new HashSet<Guid>();
        _rolesInRoleGroup.Add(message.RoleGroupId, roles);
      }
      roles.Add(message.RoleId);
    }

    public void Handle(RemovedRoleFromRoleGroup message) {
      HashSet<Guid> roles;
      if (_rolesInRoleGroup.TryGetValue(message.RoleGroupId, out roles)) {
        roles.Remove(message.RoleId);
        if (roles.Count == 0)
          _rolesInRoleGroup.Remove(message.RoleGroupId);
      }
    }

    public Guid[] GetRolesOfRoleGroup(Guid roleGroupId) {
      HashSet<Guid> roles;
      if (_rolesInRoleGroup.TryGetValue(roleGroupId, out roles))
        return roles.ToArray();
      return new Guid[0];
    }
    }
}