namespace Seabites.Naughty.Security {
  public interface IAccessDecider {
    bool IsAllowed(PermissionId permissionId);
  }
}