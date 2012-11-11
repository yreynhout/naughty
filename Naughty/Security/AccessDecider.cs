using System;
using System.Collections.Generic;
using System.Linq;

namespace Seabites.Naughty.Security {
  public class AccessDecider : IAccessDecider {
    readonly Dictionary<PermissionId, AccessDecision> _decisions;

    public AccessDecider(Dictionary<PermissionId, AccessDecision> decisions) {
      if (decisions == null) throw new ArgumentNullException("decisions");
      _decisions = decisions;
    }

    public bool AreAllAllowed(IEnumerable<PermissionId> permissionIds) {
      return permissionIds.All(IsAllowed);
    }

    public bool IsAllowed(PermissionId permissionId) {
      AccessDecision decision;
      if(_decisions.TryGetValue(permissionId, out decision)) {
        return decision == AccessDecision.Allow;
      }
      return false;
    }
  }
}