using System;
using Seabites.Naughty.Infrastructure;
using Seabites.Naughty.Messaging.Events;

namespace Seabites.Naughty.Projections {
  public interface ILookupRolesOfRoleGroup :
    IHandle<AddedRoleToRoleGroup>,
    IHandle<RemovedRoleFromRoleGroup> {
    Guid[] GetRolesOfRoleGroup(Guid roleGroupId);
  }
}