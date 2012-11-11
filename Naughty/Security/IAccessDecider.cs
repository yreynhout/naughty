using System.Collections.Generic;

namespace Seabites.Naughty.Security {
  public interface IAccessDecider {
    bool AreAllAllowed(IEnumerable<PermissionId> permissionIds);
    bool IsAllowed(PermissionId permissionId);
  }
}