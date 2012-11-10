using System;

namespace Seabites.Naughty.Infrastructure {
  public interface IAggregateRootEntity : IAggregateInitializer, IAggregateChangeTracker {
    Guid Id { get; }
    int Version { get; }
    int BaseVersion { get; }
  }
}