namespace Seabites.Naughty.Security {
  public interface IAccessDecisionCombinator {
    IAccessDecisionCombinator CombineDecision(PermissionId permissionId, AccessDecision decision);
    IAccessDecider BuildDecider();
  }
}