using System;
using AggregateSource;
using Seabites.Naughty.Infrastructure;
using Seabites.Naughty.Messaging.Commands;
using Seabites.Naughty.Security;

namespace Seabites.Naughty.Application {
  public class RoleGroupApplicationService :
    IHandle<AddRoleGroup>,
    IHandle<ArchiveRoleGroup>,
    IHandle<AddRoleToRoleGroup>,
    IHandle<RemoveRoleFromRoleGroup> {
    readonly IRepository<RoleGroup> _roleGroupRepository;
    readonly IRepository<Role> _roleRepository;

    public RoleGroupApplicationService(IRepository<RoleGroup> roleGroupRepository, IRepository<Role> roleRepository) {
      if (roleGroupRepository == null) throw new ArgumentNullException("roleGroupRepository");
      if (roleRepository == null) throw new ArgumentNullException("roleRepository");
      _roleGroupRepository = roleGroupRepository;
      _roleRepository = roleRepository;
    }

    public void Handle(AddRoleGroup message) {
      _roleGroupRepository.Add(message.RoleGroupId,
        new RoleGroup(new RoleGroupId(message.RoleGroupId), new Name(message.Name)));
    }

    public void Handle(ArchiveRoleGroup message) {
      ForRoleGroup(message.RoleGroupId).Archive();
    }

    public void Handle(AddRoleToRoleGroup message) {
      var role = _roleRepository.Get(message.RoleId);
      ForRoleGroup(message.RoleGroupId).AddRole(role);
    }

    public void Handle(RemoveRoleFromRoleGroup message) {
      var role = _roleRepository.Get(message.RoleId);
      ForRoleGroup(message.RoleGroupId).RemoveRole(role);
    }

    RoleGroup ForRoleGroup(Guid roleGroupId) {
      return _roleGroupRepository.Get(new RoleGroupId(roleGroupId));
    }
    }
}