using System;
using System.Collections.Generic;
using Seabites.Naughty.Infrastructure;
using Seabites.Naughty.Messaging.Events;

namespace Seabites.Naughty.Security {
  public class RoleGroup : AggregateRootEntity<RoleGroupId> {
    public static Func<Guid, RoleGroup> Factory = id => new RoleGroup(new RoleGroupId(id));

    RoleGroup(RoleGroupId id)
      : base(id) {
      Register<AddedRoleGroup>(Apply);
      Register<ArchivedRoleGroup>(Apply);
      Register<AddedRoleToRoleGroup>(Apply);
      Register<RemovedRoleFromRoleGroup>(Apply);
    }

    // Behavior

    public RoleGroup(RoleGroupId roleGroupId, Name name)
      : this(roleGroupId) {
      ApplyEvent(
        new AddedRoleGroup(roleGroupId, name));
    }

    public void Archive() {
      if (!_archived)
        ApplyEvent(
          new ArchivedRoleGroup(Id));
    }

    public void AddRole(Role role) {
      ThrowIfArchived();
      ApplyEvent(
        new AddedRoleToRoleGroup(Id, role.Id));
    }

    public void RemoveRole(Role role) {
      ThrowIfArchived();
      ApplyEvent(
        new RemovedRoleFromRoleGroup(Id, role.Id));
    }

    public void CombineDecisions(IAccessDecisionCombinator combinator, IRepository<Role> roleRepository) {
      if (_archived) return;
      foreach (var roleId in _roles) {
        roleRepository.
          Get(roleId).
          CombineDecisions(combinator);
      }
    }

    void ThrowIfArchived() {
      if (_archived)
        throw new Exception("Yo bro, you can't mutate this thing. It's been archived!");
    }

    // State

    HashSet<RoleId> _roles;
    bool _archived;

    void Apply(AddedRoleGroup @event) {
      _roles = new HashSet<RoleId>();
      _archived = false;
    }

    void Apply(ArchivedRoleGroup @event) {
      _archived = true;
      _roles.Clear();
    }

    void Apply(AddedRoleToRoleGroup @event) {
      _roles.Add(new RoleId(@event.RoleId));
    }

    void Apply(RemovedRoleFromRoleGroup @event) {
      _roles.Remove(new RoleId(@event.RoleId));
    }
  }
}