using System;
using System.Security;
using Seabites.Naughty.Infrastructure;
using Seabites.Naughty.Security;

namespace Seabites.Naughty {
  public class SecurityContextAwareHandler<TMessage> : IHandle<SecurityContext<TMessage>> {
    readonly IPermissionResolver _resolver;
    readonly IRepository<UserAccount> _userAccountRepository;
    readonly IRepository<Role> _roleRepository;
    readonly IRepository<RoleGroup> _roleGroupRepository;
    readonly IHandle<TMessage> _handler;

    public SecurityContextAwareHandler(
      IPermissionResolver resolver,
      IRepository<UserAccount> userAccountRepository, 
      IRepository<Role> roleRepository, 
      IRepository<RoleGroup> roleGroupRepository,
      IHandle<TMessage> handler) {
      if (resolver == null) throw new ArgumentNullException("resolver");
      if (userAccountRepository == null) throw new ArgumentNullException("userAccountRepository");
      if (roleRepository == null) throw new ArgumentNullException("roleRepository");
      if (roleGroupRepository == null) throw new ArgumentNullException("roleGroupRepository");
      _resolver = resolver;
      _userAccountRepository = userAccountRepository;
      _roleRepository = roleRepository;
      _roleGroupRepository = roleGroupRepository;
      _handler = handler;
    }

    public void Handle(SecurityContext<TMessage> context) {
      var decider = GetDeciderForUserAccount(context.UserAccountId);
      if(!decider.IsAllowed(_resolver.ResolvePermission(context.Message))) {
        throw new SecurityException(string.Format("Yo bro, u do not have permission to do {0}", context.Message.GetType()));
      }
      _handler.Handle(context.Message);
    }

    IAccessDecider GetDeciderForUserAccount(UserAccountId userAccountId) {
      var combinator = new AccessDecisionCombinator();
      var userAccount = _userAccountRepository.Get(userAccountId);
      userAccount.CombineDecisions(combinator, _roleRepository, _roleGroupRepository);
      return combinator.BuildDecider();
    }
  }
}