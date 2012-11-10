using System;
using System.Collections.Generic;

namespace Seabites.Naughty.Security {
  public class AccessDecisionCombinator : IAccessDecisionCombinator {
    readonly Dictionary<PermissionId, AccessDecision> _decisions;

    static readonly Dictionary<Tuple<AccessDecision, AccessDecision>, AccessDecision> Combinations =
      new Dictionary<Tuple<AccessDecision, AccessDecision>, AccessDecision> {
        { new Tuple<AccessDecision, AccessDecision>(AccessDecision.Deny, AccessDecision.Allow), AccessDecision.Deny },
        { new Tuple<AccessDecision, AccessDecision>(AccessDecision.Deny, AccessDecision.Deny), AccessDecision.Deny },
        { new Tuple<AccessDecision, AccessDecision>(AccessDecision.Deny, AccessDecision.Indeterminate), AccessDecision.Deny },

        { new Tuple<AccessDecision, AccessDecision>(AccessDecision.Indeterminate, AccessDecision.Allow), AccessDecision.Allow },
        { new Tuple<AccessDecision, AccessDecision>(AccessDecision.Indeterminate, AccessDecision.Deny), AccessDecision.Deny },
        { new Tuple<AccessDecision, AccessDecision>(AccessDecision.Indeterminate, AccessDecision.Indeterminate), AccessDecision.Indeterminate },

        { new Tuple<AccessDecision, AccessDecision>(AccessDecision.Allow, AccessDecision.Allow), AccessDecision.Allow },
        { new Tuple<AccessDecision, AccessDecision>(AccessDecision.Allow, AccessDecision.Deny), AccessDecision.Deny },
        { new Tuple<AccessDecision, AccessDecision>(AccessDecision.Allow, AccessDecision.Indeterminate), AccessDecision.Indeterminate },
      };

    public AccessDecisionCombinator() {
      _decisions = new Dictionary<PermissionId, AccessDecision>();
    }

    public IAccessDecisionCombinator CombineDecision(PermissionId permissionId, AccessDecision decision) {
      AccessDecision otherDecision;
      if(_decisions.TryGetValue(permissionId, out otherDecision)) {
        _decisions[permissionId] = Combinations[new Tuple<AccessDecision, AccessDecision>(otherDecision, decision)];
      } else {
        _decisions[permissionId] = decision;
      }
      return this;
    }

    public IAccessDecider BuildDecider() {
      return new AccessDecider(_decisions);
    }
  }
}