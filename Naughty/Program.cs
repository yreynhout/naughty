using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Seabites.Naughty.Application;
using Seabites.Naughty.Infrastructure;
using Seabites.Naughty.Messaging.Commands;
using Seabites.Naughty.Messaging.Events;
using Seabites.Naughty.Projections;
using Seabites.Naughty.Security;

namespace Seabites.Naughty {
  class Program {
    static void Main() {
      var storage = new Dictionary<Guid, List<object>>();
      var reader = new EventStreamReader(storage);
      var unitOfWork = new UnitOfWork();

      var roleRepository = new Repository<Role>(Role.Factory, reader, unitOfWork);
      var roleGroupRepository = new Repository<RoleGroup>(RoleGroup.Factory, reader, unitOfWork);
      var userAccountRepository = new Repository<UserAccount>(UserAccount.Factory, reader, unitOfWork);

      // Setting up security (accounts, roles and authorization)
      var administratorRoleId = new RoleId(Guid.NewGuid());
      var administratorRole = new Role(administratorRoleId, new Name("Administrator"));
      administratorRole.AddPermissions(SecurityPermissions.All);
      administratorRole.AllowPermissions(SecurityPermissions.All);
      roleRepository.Add(administratorRole);
      var subRole1Id = new RoleId(Guid.NewGuid());
      var subRole1 = new Role(subRole1Id, new Name("SubRole1"));
      subRole1.AddPermission(SecurityPermissions.AddRole);
      subRole1.DenyPermission(SecurityPermissions.AddRole);
      roleRepository.Add(subRole1);
      var subRole2Id = new RoleId(Guid.NewGuid());
      var subRole2 = new Role(subRole2Id, new Name("SubRole2"));
      subRole2.AddPermission(SecurityPermissions.AddRole);
      subRole2.AllowPermission(SecurityPermissions.AddRole);
      roleRepository.Add(subRole2);
      var group1Id = new RoleGroupId(Guid.NewGuid());
      var group1 = new RoleGroup(group1Id, new Name("SubRole 1 & 2"));
      group1.AddRole(subRole1);
      group1.AddRole(subRole2);
      roleGroupRepository.Add(group1);

      var administratorId = new UserAccountId(Guid.NewGuid());
      var administrator = new UserAccount(administratorId, new UserAccountName("Administrator"));
      administrator.GrantRole(administratorRole);
      administrator.GrantRoleGroup(group1);
      userAccountRepository.Add(administrator);

      // Using security - in domain layer code
      var combinator = new AccessDecisionCombinator();
      administrator.CombineDecisions(combinator, roleRepository, roleGroupRepository);
      var decider = combinator.BuildDecider();

      Console.WriteLine(decider.IsAllowed(SecurityPermissions.AddUserAccount));

      // Using security - in application layer code

      var command = new AddUserAccount(
        new Guid("735A259F-996B-4174-9899-3D40242BF6B1"),
        "Pierke Pol");
      var resolver = new PermissionResolver();
      var service =
        new UserAccountApplicationService(
          userAccountRepository,
          roleRepository,
          roleGroupRepository);
      var authorizer =
        new MessageAuthorizer(
          resolver,
          userAccountRepository,
          roleRepository,
          roleGroupRepository);
      var commandHandler = service.Secure<AddUserAccount>(authorizer);
      commandHandler.Handle(new SecurityContext<AddUserAccount>(administratorId, command));

      // Using security - in projection code
      var builder = new SqlConnectionStringBuilder("Data Source=.\\SQLEXPRESS;Initial Catalog=<YourStoreHere>;Integrated Security=SSPI;");

      // What that table could look like ...
      //CREATE TABLE [UserAccountEffectiveRoles](
      //  [UserAccountId] [uniqueidentifier] NOT NULL,
      //  [RoleId] [uniqueidentifier] NULL,
      //  [RoleGroupId] [uniqueidentifier] NULL,
      //  [Id] [int] IDENTITY(1,1) NOT NULL,
      //  CONSTRAINT [PK_UserAccountEffectiveRoles] PRIMARY KEY CLUSTERED ( [Id] ASC )
      //)

      var observer = new SqlStatementObserver();
      // var lookup = new MemoryRoleGroupLookup(new Dictionary<Guid, HashSet<Guid>>());
      var lookupInitializer = new SqlBasedLookupRolesOfRoleGroupInitializer(builder);
      var lookup = lookupInitializer.Initialize();
      var projectionHandler = new UserAccountEffectiveRolesProjectionHandler(observer, lookup);
      var compositeProjectionHandler = new CompositeHandler(
        new IHandle<object>[] {
          new HandlerAdapter<AddedRoleToRoleGroup>(lookup),
          new HandlerAdapter<RemovedRoleFromRoleGroup>(lookup),
          new HandlerAdapter<DisabledUserAccount>(projectionHandler),
          new HandlerAdapter<RoleGrantedToUserAccount>(projectionHandler),
          new HandlerAdapter<RoleRevokedFromUserAccount>(projectionHandler),
          new HandlerAdapter<RoleGroupGrantedToUserAccount>(projectionHandler),
          new HandlerAdapter<RoleRevokedFromUserAccount>(projectionHandler),
          new HandlerAdapter<AddedRoleToRoleGroup>(projectionHandler),
          new HandlerAdapter<RemovedRoleFromRoleGroup>(projectionHandler)
        });
      foreach (var change in unitOfWork.GetChanges()) {
        compositeProjectionHandler.Handle(change);
      }

      new BatchedSqlStatementFlusher(
        builder).
        Flush(observer.Statements);

      Console.ReadLine();
    }
  }
}