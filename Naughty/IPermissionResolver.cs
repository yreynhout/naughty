using Seabites.Naughty.Security;

namespace Seabites.Naughty {
  public interface IPermissionResolver {
    PermissionId ResolvePermission(object message);
  }
}