using System;
using Seabites.Naughty.Infrastructure;
using Seabites.Naughty.Messaging.Commands;
using Seabites.Naughty.Security;

namespace Seabites.Naughty.Application {
  public class UserAccountApplicationService :
    IHandle<AddUserAccount>,
    IHandle<DisableUserAccount>,
    IHandle<GrantRoleGroupToUserAccount>,
    IHandle<GrantRoleToUserAccount>,
    IHandle<RevokeRoleFromUserAccount>,
    IHandle<RevokeRoleGroupFromUserAccount> {
    readonly IRepository<UserAccount> _userAccountRepository;
    readonly IRepository<Role> _roleRepository;
    readonly IRepository<RoleGroup> _roleGroupRepository;

    public UserAccountApplicationService(IRepository<UserAccount> userAccountRepository,
                                         IRepository<Role> roleRepository, IRepository<RoleGroup> roleGroupRepository) {
      if (userAccountRepository == null) throw new ArgumentNullException("userAccountRepository");
      if (roleRepository == null) throw new ArgumentNullException("roleRepository");
      if (roleGroupRepository == null) throw new ArgumentNullException("roleGroupRepository");
      _userAccountRepository = userAccountRepository;
      _roleRepository = roleRepository;
      _roleGroupRepository = roleGroupRepository;
    }

    public void Handle(AddUserAccount message) {
      _userAccountRepository.Add(
        new UserAccount(
          new UserAccountId(message.UserAccountId),
          new UserAccountName(message.Name)));
    }

    public void Handle(DisableUserAccount message) {
      ForUserAccount(message.UserAccountId).Disable();
    }

    public void Handle(GrantRoleToUserAccount message) {
      var role = _roleRepository.Get(message.RoleId);
      ForUserAccount(message.UserAccountId).GrantRole(role);
    }

    public void Handle(GrantRoleGroupToUserAccount message) {
      var roleGroup = _roleGroupRepository.Get(message.RoleGroupId);
      ForUserAccount(message.UserAccountId).GrantRoleGroup(roleGroup);
    }

    public void Handle(RevokeRoleFromUserAccount message) {
      var role = _roleRepository.Get(message.RoleId);
      ForUserAccount(message.UserAccountId).RevokeRole(role);
    }

    public void Handle(RevokeRoleGroupFromUserAccount message) {
      var roleGroup = _roleGroupRepository.Get(message.RoleGroupId);
      ForUserAccount(message.UserAccountId).RevokeRoleGroup(roleGroup);
    }

    UserAccount ForUserAccount(Guid userAccountId) {
      return _userAccountRepository.Get(new UserAccountId(userAccountId));
    }
    }
}