using System;

namespace Seabites.Naughty.Infrastructure {
  public interface IAggregateIdentity {
    Guid Value { get; }
  }
}