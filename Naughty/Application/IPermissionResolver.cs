using System.Collections.Generic;
using Seabites.Naughty.Security;

namespace Seabites.Naughty.Application {
  public interface IPermissionResolver {
    IEnumerable<PermissionId> ResolvePermission(object message);
  }
}