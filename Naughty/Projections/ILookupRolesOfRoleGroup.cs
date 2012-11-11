using System;

namespace Seabites.Naughty.Projections {
  public interface ILookupRolesOfRoleGroup {
    Guid[] GetRolesOfRoleGroup(Guid roleGroupId);
  }
}