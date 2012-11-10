using System;
using System.Collections.Generic;
using Seabites.Naughty.Infrastructure;
using Seabites.Naughty.Messaging;
using Seabites.Naughty.Security;

namespace Seabites.Naughty {
  class Program {
    static void Main() {
      var storage = new Dictionary<Guid, List<object>>();
      var reader = new EventStreamReader(storage);
      var unitOfWork =new UnitOfWork();
      
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

      // Using security
      var combinator = new AccessDecisionCombinator();
      administrator.CombineDecisions(combinator, roleRepository, roleGroupRepository);
      var decider = combinator.BuildDecider();

      Console.WriteLine(decider.IsAllowed(SecurityPermissions.AddUserAccount));

      var command = new AddUserAccount(
        new Guid("735A259F-996B-4174-9899-3D40242BF6B1"), 
        "Pierke Pol");
      var resolver = new PermissionResolver();
      var handler = new SecurityContextAwareHandler<AddUserAccount>(
        resolver, userAccountRepository, roleRepository, roleGroupRepository,
        new AddUserAccountHandler(userAccountRepository));
      handler.Handle(new SecurityContext<AddUserAccount>(command, administratorId));

      Console.ReadLine();
    }
  }
}
