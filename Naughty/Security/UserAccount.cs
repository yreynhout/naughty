using System;
using System.Collections.Generic;
using AggregateSource;
using Seabites.Naughty.Messaging.Events;

namespace Seabites.Naughty.Security {
  public class UserAccount : AggregateRootEntity {
    public static readonly Func<UserAccount> Factory = () => new UserAccount();

    UserAccount() {
      Register<AddedUserAccount>(When);
      Register<DisabledUserAccount>(When);
      Register<RoleGrantedToUserAccount>(When);
      Register<RoleRevokedFromUserAccount>(When);
      Register<RoleGroupGrantedToUserAccount>(When);
      Register<RoleGroupRevokedFromUserAccount>(When);
    }

    public UserAccountId Id { get; private set; }

    // Behavior

    public UserAccount(UserAccountId userAccountId, UserAccountName name) : this() {
      Apply(
        new AddedUserAccount(userAccountId, name));
    }

    public void GrantRole(Role role) {
      ThrowIfDisabled();
      Apply(
        new RoleGrantedToUserAccount(Id, role.Id));
    }

    public void GrantRoleGroup(RoleGroup roleGroup) {
      ThrowIfDisabled();
      Apply(
        new RoleGroupGrantedToUserAccount(Id, roleGroup.Id));
    }

    public void RevokeRole(Role role) {
      ThrowIfDisabled();
      Apply(
        new RoleRevokedFromUserAccount(Id, role.Id));
    }

    public void RevokeRoleGroup(RoleGroup roleGroup) {
      ThrowIfDisabled();
      Apply(
        new RoleGroupRevokedFromUserAccount(Id, roleGroup.Id));
    }

    public void Disable() {
      if (!_disabled)
        Apply(
          new DisabledUserAccount(Id));
    }

    public void CombineDecisions(IAccessDecisionCombinator combinator, IRepository<Role> roleRepository,
                                 IRepository<RoleGroup> roleGroupRepository) {
      if (_disabled) return;

      foreach (var roleId in _roles) {
        roleRepository.Get(roleId).CombineDecisions(combinator);
      }

      foreach (var roleGroupId in _roleGroups) {
        roleGroupRepository.Get(roleGroupId).CombineDecisions(combinator, roleRepository);
      }
    }

    void ThrowIfDisabled() {
      if (_disabled)
        throw new Exception("Yo bro, you can't mutate this thing. It's been disabled!");
    }

    // State

    HashSet<RoleId> _roles;
    HashSet<RoleGroupId> _roleGroups;
    bool _disabled;

    void When(AddedUserAccount @event) {
      Id = new UserAccountId(@event.UserAccountId);
      _roles = new HashSet<RoleId>();
      _roleGroups = new HashSet<RoleGroupId>();
      _disabled = false;
    }

    void When(DisabledUserAccount @event) {
      _disabled = true;
      _roles.Clear();
      _roleGroups.Clear();
    }

    void When(RoleGrantedToUserAccount @event) {
      _roles.Add(new RoleId(@event.RoleId));
    }

    void When(RoleRevokedFromUserAccount @event) {
      _roles.Remove(new RoleId(@event.RoleId));
    }

    void When(RoleGroupGrantedToUserAccount @event) {
      _roleGroups.Add(new RoleGroupId(@event.RoleGroupId));
    }

    void When(RoleGroupRevokedFromUserAccount @event) {
      _roleGroups.Remove(new RoleGroupId(@event.RoleGroupId));
    }
  }
}