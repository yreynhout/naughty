using System;
using System.Collections.Generic;
using AggregateSource;
using Seabites.Naughty.Messaging.Events;

namespace Seabites.Naughty.Security {
  public class RoleGroup : AggregateRootEntity {
    public static readonly Func<RoleGroup> Factory = () => new RoleGroup();

    RoleGroup() {
      Register<AddedRoleGroup>(When);
      Register<ArchivedRoleGroup>(When);
      Register<AddedRoleToRoleGroup>(When);
      Register<RemovedRoleFromRoleGroup>(When);
    }

    public RoleGroupId Id { get; private set; }

    // Behavior

    public RoleGroup(RoleGroupId roleGroupId, Name name) : this() {
      Apply(
        new AddedRoleGroup(roleGroupId, name));
    }

    public void Archive() {
      if (!_archived)
        Apply(
          new ArchivedRoleGroup(Id));
    }

    public void AddRole(Role role) {
      ThrowIfArchived();
      Apply(
        new AddedRoleToRoleGroup(Id, role.Id));
    }

    public void RemoveRole(Role role) {
      ThrowIfArchived();
      Apply(
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

    void When(AddedRoleGroup @event) {
      Id = new RoleGroupId(@event.RoleGroupId);
      _roles = new HashSet<RoleId>();
      _archived = false;
    }

    void When(ArchivedRoleGroup @event) {
      _archived = true;
      _roles.Clear();
    }

    void When(AddedRoleToRoleGroup @event) {
      _roles.Add(new RoleId(@event.RoleId));
    }

    void When(RemovedRoleFromRoleGroup @event) {
      _roles.Remove(new RoleId(@event.RoleId));
    }
  }
}