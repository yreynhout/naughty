using System;
using System.Collections.Generic;
using Seabites.Naughty.Infrastructure;
using Seabites.Naughty.Messaging.Events;

namespace Seabites.Naughty.Security {
  public class UserAccount : AggregateRootEntity<UserAccountId> {
    public static Func<Guid, UserAccount> Factory = id => new UserAccount(new UserAccountId(id));

    UserAccount(UserAccountId id) : base(id) {
      Register<AddedUserAccount>(Apply);
      Register<DisabledUserAccount>(Apply);
      Register<RoleGrantedToUserAccount>(Apply);
      Register<RoleRevokedFromUserAccount>(Apply);
      Register<RoleGroupGrantedToUserAccount>(Apply);
      Register<RoleGroupRevokedFromUserAccount>(Apply);
    }

    // Behavior

    public UserAccount(UserAccountId userAccountId, UserAccountName name)
      : this(userAccountId) {
      ApplyEvent(
        new AddedUserAccount(userAccountId, name));
    }

    public void GrantRole(Role role) {
      ThrowIfDisabled();
      ApplyEvent(
        new RoleGrantedToUserAccount(Id, role.Id));
    }

    public void GrantRoleGroup(RoleGroup roleGroup) {
      ThrowIfDisabled();
      ApplyEvent(
        new RoleGroupGrantedToUserAccount(Id, roleGroup.Id));
    }

    public void RevokeRole(Role role) {
      ThrowIfDisabled();
      ApplyEvent(
        new RoleRevokedFromUserAccount(Id, role.Id));
    }

    public void RevokeRoleGroup(RoleGroup roleGroup) {
      ThrowIfDisabled();
      ApplyEvent(
        new RoleGroupRevokedFromUserAccount(Id, roleGroup.Id));
    }

    public void Disable() {
      if (!_disabled)
        ApplyEvent(
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

    void Apply(AddedUserAccount @event) {
      _roles = new HashSet<RoleId>();
      _roleGroups = new HashSet<RoleGroupId>();
      _disabled = false;
    }

    void Apply(DisabledUserAccount @event) {
      _disabled = true;
      _roles.Clear();
      _roleGroups.Clear();
    }

    void Apply(RoleGrantedToUserAccount @event) {
      _roles.Add(new RoleId(@event.RoleId));
    }

    void Apply(RoleRevokedFromUserAccount @event) {
      _roles.Remove(new RoleId(@event.RoleId));
    }

    void Apply(RoleGroupGrantedToUserAccount @event) {
      _roleGroups.Add(new RoleGroupId(@event.RoleGroupId));
    }

    void Apply(RoleGroupRevokedFromUserAccount @event) {
      _roleGroups.Remove(new RoleGroupId(@event.RoleGroupId));
    }
  }
}