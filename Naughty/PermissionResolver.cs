using Seabites.Naughty.Messaging;
using Seabites.Naughty.Security;

namespace Seabites.Naughty {
  public class PermissionResolver : IPermissionResolver {
    public PermissionId ResolvePermission(object message) {
      if(message is AddUserAccount)
        return SecurityPermissions.AddUserAccount;
      return PermissionId.None;
    }
  }
}