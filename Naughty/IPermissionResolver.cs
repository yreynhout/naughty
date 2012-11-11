using System.Collections.Generic;
using Seabites.Naughty.Security;

namespace Seabites.Naughty {
  public interface IPermissionResolver {
    IEnumerable<PermissionId> ResolvePermission(object message);
  }
}