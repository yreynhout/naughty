using System;
using Seabites.Naughty.Infrastructure;
using Seabites.Naughty.Messaging;

namespace Seabites.Naughty.Projections {
  public class UserAccountEffectiveRolesProjectionHandler
    : IHandle<DisabledUserAccount>,
    IHandle<RoleGrantedToUserAccount>,
    IHandle<RoleGroupGrantedToUserAccount>,
    IHandle<RoleGroupRevokedFromUserAccount>,
    IHandle<RoleRevokedFromUserAccount>,
    IHandle<AddedRoleToRoleGroup>,
    IHandle<RemovedRoleFromRoleGroup> {
    readonly IObserver<ISqlStatement> _observer;
    readonly ILookupRolesOfRoleGroup _lookup;
    readonly Random _random;

    public UserAccountEffectiveRolesProjectionHandler(IObserver<ISqlStatement> observer, ILookupRolesOfRoleGroup lookup) {
      if (observer == null) throw new ArgumentNullException("observer");
      if (lookup == null) throw new ArgumentNullException("lookup");
      _observer = observer;
      _lookup = lookup;
      _random = new Random();
    }

    public void Handle(DisabledUserAccount message) {
      _observer.OnNext(
        new SqlTextStatement(
          "DELETE FROM [UserAccountEffectiveRoles] WHERE [UserAccountId] = @UserAccountId",
          new { UserAccountId = message.Id }));
    }

    public void Handle(RoleGrantedToUserAccount message) {
      _observer.OnNext(
        new SqlTextStatement(
          "IF NOT EXISTS(SELECT 1 FROM [UserAccountEffectiveRoles] WHERE [UserAccountId] = @UserAccountId AND [RoleId] = @RoleId AND [RoleGroupId] IS NULL) " +
          "BEGIN" +
          "  INSERT INTO [UserAccountEffectiveRoles] ([UserAccountId],[RoleId],[RoleGroupId]) VALUES (@UserAccountId,@RoleId,NULL) " +
          "END",
          new { UserAccountId = message.UserAccountId, RoleId = message.RoleId }));
    }

    public void Handle(RoleRevokedFromUserAccount message) {
      _observer.OnNext(
        new SqlTextStatement(
          "DELETE FROM [UserAccountEffectiveRoles] WHERE [UserAccountId] = @UserAccountId AND [RoleId] = @RoleId AND [RoleGroupId] IS NULL)",
          new { UserAccountId = message.UserAccountId, RoleId = message.RoleId }));
    }

    public void Handle(RoleGroupGrantedToUserAccount message) {
      foreach (var roleId in _lookup.GetRolesOfRoleGroup(message.RoleGroupId)) {
        _observer.OnNext(
          new SqlTextStatement(
            "INSERT INTO [UserAccountEffectiveRoles] ([UserAccountId],[RoleId],[RoleGroupId]) VALUES (@UserAccountId,@RoleId,@RoleGroupId)",
            new {UserAccountId = message.UserAccountId, RoleId = roleId, RoleGroupId = message.RoleGroupId}));
      }
      // role group marker row - this would be the kind of row you'd need to skip if you wanted to get the effective roles
      _observer.OnNext(
          new SqlTextStatement(
            "INSERT INTO [UserAccountEffectiveRoles] ([UserAccountId],[RoleId],[RoleGroupId]) VALUES (@UserAccountId,NULL,@RoleGroupId)",
            new { UserAccountId = message.UserAccountId, RoleGroupId = message.RoleGroupId }));
    }

    public void Handle(RoleGroupRevokedFromUserAccount message) {
      _observer.OnNext(
        new SqlTextStatement(
          "DELETE FROM [UserAccountEffectiveRoles] WHERE [UserAccountId] = @UserAccountId AND [RoleGroupId] = @RoleGroupId)",
          new { UserAccountId = message.UserAccountId, RoleGroupId = message.RoleGroupId }));
    }

    public void Handle(AddedRoleToRoleGroup message) {
      var tempTableName = "#AffectedUserAccounts" + _random.Next();
      _observer.OnNext(
        new SqlTextStatement(
          string.Format(
          "CREATE TABLE {0} (UserAccountId UNIQUEIDENTIFIER) " + Environment.NewLine +
          "INSERT INTO {0} " + Environment.NewLine +
          "SELECT UserAccountId FROM UserAccountEffectiveRoles WHERE RoleGroupId = @RoleGroupId " + Environment.NewLine +
          "INSERT INTO UserAccountEffectiveRoles (UserAccountId, RoleGroupId, RoleId) " + Environment.NewLine +
          "SELECT UserAccountId, @RoleGroupId, @RoleId FROM {0} " + Environment.NewLine +
          "DROP TABLE {0}",
          tempTableName
          ),
          new {RoleGroupId = message.RoleGroupId, RoleId = message.RoleId}));
    }

    public void Handle(RemovedRoleFromRoleGroup message) {
      _observer.OnNext(
        new SqlTextStatement(
          "DELETE FROM UserAccountEffectiveRoles WHERE RoleGroupId = @RoleGroupId AND RoleId = @RoleId",
          new { RoleGroupId = message.RoleGroupId, RoleId = message.RoleId }));
    }
  }
}
