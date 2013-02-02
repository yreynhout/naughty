using System;
using System.Collections.Generic;
using AggregateSource;
using Seabites.Naughty.Messaging.Events;

namespace Seabites.Naughty.Security {
  public class Role : AggregateRootEntity {
    public static readonly Func<Role> Factory = () => new Role();

    Role() {
      Register<AddedRole>(When);
      Register<AddedPermissionToRole>(When);
      Register<RemovedPermissionFromRole>(When);
      Register<RolePermissionAllowed>(When);
      Register<RolePermissionDenied>(When);
      Register<ArchivedRole>(When);
    }

    public RoleId Id { get; private set; }

    // Behavior

    public Role(RoleId roleId, Name name) : this() {
      Apply(
        new AddedRole(roleId, name));
    }

    public void Archive() {
      if (!_archived)
        Apply(
          new ArchivedRole(Id));
    }

    public void AddPermissions(IEnumerable<PermissionId> permissionIds) {
      foreach (var permissionId in permissionIds) {
        AddPermission(permissionId);
      }
    }

    public void AddPermission(PermissionId permissionId) {
      ThrowIfArchived();
      if (IsUnknownPermission(permissionId)) {
        Apply(
          new AddedPermissionToRole(Id, permissionId));
      }
    }

    public void AllowPermissions(IEnumerable<PermissionId> permissionIds) {
      foreach (var permissionId in permissionIds) {
        AllowPermission(permissionId);
      }
    }

    public void AllowPermission(PermissionId permissionId) {
      ThrowIfArchived();
      if (IsKnownPermission(permissionId)) {
        Apply(
          new RolePermissionAllowed(Id, permissionId));
      }
    }

    public void DenyPermission(PermissionId permissionId) {
      ThrowIfArchived();
      if (IsKnownPermission(permissionId)) {
        Apply(
          new RolePermissionDenied(Id, permissionId));
      }
    }

    public void RemovePermission(PermissionId permissionId) {
      ThrowIfArchived();
      if (IsKnownPermission(permissionId)) {
        Apply(
          new RemovedPermissionFromRole(Id, permissionId));
      }
    }

    public void CombineDecisions(IAccessDecisionCombinator combinator) {
      if (_archived) return;
      foreach (var permission in _permissions) {
        permission.CombineDecision(combinator);
      }
    }

    void ThrowIfArchived() {
      if (_archived)
        throw new Exception("Yo bro, you can't mutate this thing. It's been archived!");
    }

    bool IsUnknownPermission(PermissionId permissionId) {
      return !_permissions.Exists(permission => permission.PermissionId == permissionId);
    }

    bool IsKnownPermission(PermissionId permissionId) {
      return _permissions.Exists(permission => permission.PermissionId == permissionId);
    }

    // State

    List<RolePermission> _permissions;
    bool _archived;

    void When(AddedRole @event) {
      Id = new RoleId(@event.RoleId);
      _permissions = new List<RolePermission>();
      _archived = false;
    }

    void When(ArchivedRole @event) {
      _archived = true;
      _permissions.Clear();
    }

    void When(AddedPermissionToRole @event) {
      _permissions.Add(
        new RolePermission(
          new PermissionId(@event.PermissionId),
          AccessDecision.Indeterminate));
    }

    void When(RemovedPermissionFromRole @event) {
      _permissions.Remove(
        FindPermission(new PermissionId(@event.PermissionId)));
    }

    void When(RolePermissionDenied @event) {
      FindPermission(new PermissionId(@event.PermissionId)).Deny();
    }

    void When(RolePermissionAllowed @event) {
      FindPermission(new PermissionId(@event.PermissionId)).Allow();
    }

    RolePermission FindPermission(PermissionId permissionId) {
      return _permissions.Find(permission => permission.PermissionId == permissionId);
    }

    class RolePermission {
      readonly PermissionId _permissionId;
      AccessDecision _accessDecision;

      public RolePermission(PermissionId permissionId, AccessDecision accessDecision) {
        _permissionId = permissionId;
        _accessDecision = accessDecision;
      }

      public PermissionId PermissionId {
        get { return _permissionId; }
      }

      public void Allow() {
        _accessDecision = AccessDecision.Allow;
      }

      public void Deny() {
        _accessDecision = AccessDecision.Deny;
      }

      public void CombineDecision(IAccessDecisionCombinator combinator) {
        combinator.CombineDecision(_permissionId, _accessDecision);
      }
    }
  }
}