using System;
using AggregateSource;
using Seabites.Naughty.Infrastructure;
using Seabites.Naughty.Messaging.Commands;
using Seabites.Naughty.Security;

namespace Seabites.Naughty.Application {
  public class RoleApplicationService :
    IHandle<AddRole>,
    IHandle<AddPermissionToRole>,
    IHandle<RemovePermissionFromRole>,
    IHandle<ArchiveRole>,
    IHandle<AllowRolePermission>,
    IHandle<DenyRolePermission> {
    readonly IRepository<Role> _roleRepository;

    public RoleApplicationService(IRepository<Role> roleRepository) {
      if (roleRepository == null) throw new ArgumentNullException("roleRepository");
      _roleRepository = roleRepository;
    }

    public void Handle(AddRole message) {
      _roleRepository.Add(message.RoleId,
        new Role(new RoleId(message.RoleId), new Name(message.Name)));
    }

    public void Handle(ArchiveRole message) {
      ForRole(message.RoleId).Archive();
    }

    public void Handle(AllowRolePermission message) {
      ForRole(message.RoleId).AllowPermission(new PermissionId(message.PermissionId));
    }

    public void Handle(DenyRolePermission message) {
      ForRole(message.RoleId).DenyPermission(new PermissionId(message.PermissionId));
    }

    public void Handle(AddPermissionToRole message) {
      ForRole(message.RoleId).AddPermission(new PermissionId(message.PermissionId));
    }

    public void Handle(RemovePermissionFromRole message) {
      ForRole(message.RoleId).RemovePermission(new PermissionId(message.PermissionId));
    }

    Role ForRole(Guid roleId) {
      return _roleRepository.Get(new RoleId(roleId));
    }
    }
}