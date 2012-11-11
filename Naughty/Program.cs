using System;
using System.Collections.Generic;
using Seabites.Naughty.Infrastructure;
using Seabites.Naughty.Messaging;
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
      
      var administratorId = new UserAccountId(Guid.NewGuid());
      var administrator = new UserAccount(administratorId, new UserAccountName("Administrator"));
      administrator.GrantRole(administratorRoleId);
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
      var commandHandler = new SecurityContextAwareHandler<AddUserAccount>(
        resolver, userAccountRepository, roleRepository, roleGroupRepository,
        new AddUserAccountHandler(userAccountRepository));
      commandHandler.Handle(new SecurityContext<AddUserAccount>(command, administratorId));

      // Using security - in projection code

      var observer = new SqlStatementObserver();
      var lookup = new MemoryRoleGroupLookup();
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
      foreach(var change in unitOfWork.GetChanges()) {
        compositeProjectionHandler.Handle(change);
      }

      foreach(var statement in observer.Statements) {
        statement.WriteSql(Console.Out);
      }
      Console.ReadLine();
    }
  }
}
