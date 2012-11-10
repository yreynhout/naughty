using System;
using System.Collections.Generic;
using Seabites.Naughty.Infrastructure;
using Seabites.Naughty.Messaging;

namespace Seabites.Naughty.Security {
  public class Role : AggregateRootEntity<RoleId> {
    public static Func<Guid, Role> Factory = id => new Role(new RoleId(id));

    Role(RoleId id) : base(id) {
      Register<AddedRole>(Apply);
      Register<AddedPermissionToRole>(Apply);
      Register<RemovedPermissionFromRole>(Apply);
      Register<RolePermissionAllowed>(Apply);
      Register<RolePermissionDenied>(Apply);
      Register<ArchivedRole>(Apply);
    }

    // Behavior

    public Role(RoleId roleId, Name name) 
      : this(roleId) {
      ApplyEvent(
        new AddedRole(roleId, name));
    }

    public void Archive() {
      if (!_archived)
        ApplyEvent(
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
        ApplyEvent(
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
        ApplyEvent(
          new RolePermissionAllowed(Id, permissionId));
      }
    }

    public void DenyPermission(PermissionId permissionId) {
      ThrowIfArchived();
      if (IsKnownPermission(permissionId)) {
        ApplyEvent(
          new RolePermissionDenied(Id, permissionId));
      }
    }

    public void RemovePermission(PermissionId permissionId) {
      ThrowIfArchived();
      if (IsKnownPermission(permissionId)) {
        ApplyEvent(
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

    void Apply(AddedRole @event) {
      _permissions = new List<RolePermission>();
      _archived = false;
    }

    void Apply(ArchivedRole @event) {
      _archived = true;
      _permissions.Clear();
    }

    void Apply(AddedPermissionToRole @event) {
      _permissions.Add(
        new RolePermission(
          new PermissionId(@event.PermissionId), 
          AccessDecision.Indeterminate));
    }

    void Apply(RemovedPermissionFromRole @event) {
      _permissions.Remove(
        FindPermission(new PermissionId(@event.PermissionId)));
    }

    void Apply(RolePermissionDenied @event) {
      FindPermission(new PermissionId(@event.PermissionId)).Deny();
    }

    void Apply(RolePermissionAllowed @event) {
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