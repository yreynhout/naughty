using System;
using Seabites.Naughty.Infrastructure;
using Seabites.Naughty.Messaging;
using Seabites.Naughty.Security;

namespace Seabites.Naughty {
  public class AddUserAccountHandler : IHandle<AddUserAccount> {
    readonly IRepository<UserAccount> _userAccountRepository;

    public AddUserAccountHandler(IRepository<UserAccount> userAccountRepository) {
      if (userAccountRepository == null) throw new ArgumentNullException("userAccountRepository");
      _userAccountRepository = userAccountRepository;
    }

    public void Handle(AddUserAccount message) {
      _userAccountRepository.Add(
        new UserAccount(new UserAccountId(message.UserAccountId), new UserAccountName(message.Name)));
    }
  }
}