using System;

namespace Seabites.Naughty.Messaging {
  public class DisabledUserAccount {
    public readonly Guid Id;
    public DisabledUserAccount(Guid id) {
      Id = id;
    }
  }
}