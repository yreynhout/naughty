using System.Collections.Generic;
using Seabites.Naughty.Messaging;
using Seabites.Naughty.Security;

namespace Seabites.Naughty {
  public class PermissionResolver : IPermissionResolver {
    public IEnumerable<PermissionId> ResolvePermission(object message) {
      if(message is AddUserAccount)
        yield return SecurityPermissions.AddUserAccount;
    }
  }
}