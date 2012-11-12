using System;
using System.Security;
using Seabites.Naughty.Infrastructure;
using Seabites.Naughty.Security;

namespace Seabites.Naughty.Application {
  public class MessageAuthorizer : IMessageAuthorizer {
    readonly IPermissionResolver _resolver;
    readonly IRepository<UserAccount> _userAccountRepository;
    readonly IRepository<Role> _roleRepository;
    readonly IRepository<RoleGroup> _roleGroupRepository;

    public MessageAuthorizer(IPermissionResolver resolver,
                             IRepository<UserAccount> userAccountRepository,
                             IRepository<Role> roleRepository,
                             IRepository<RoleGroup> roleGroupRepository) {
      if (resolver == null) throw new ArgumentNullException("resolver");
      if (userAccountRepository == null) throw new ArgumentNullException("userAccountRepository");
      if (roleRepository == null) throw new ArgumentNullException("roleRepository");
      if (roleGroupRepository == null) throw new ArgumentNullException("roleGroupRepository");
      _resolver = resolver;
      _userAccountRepository = userAccountRepository;
      _roleRepository = roleRepository;
      _roleGroupRepository = roleGroupRepository;
    }

    public void Authorize(UserAccountId account, object message) {
      var decider = GetDeciderForMessage(account);
      if (!decider.AreAllAllowed(_resolver.ResolvePermission(message))) {
        throw new SecurityException(string.Format("Yo bro, u do not have permission to do {0}", message.GetType()));
      }
    }

    IAccessDecider GetDeciderForMessage(UserAccountId account) {
      var combinator = new AccessDecisionCombinator();
      var userAccount = _userAccountRepository.Get(account);
      userAccount.CombineDecisions(combinator, _roleRepository, _roleGroupRepository);
      return combinator.BuildDecider();
    }
  }
}