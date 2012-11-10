using System;
using Seabites.Naughty.Infrastructure;

namespace Seabites.Naughty.Security {
  public struct RoleId : IEquatable<RoleId>, IAggregateIdentity {
    readonly Guid _value;

    public RoleId(Guid value) {
      _value = value;
    }

    public static bool operator ==(RoleId left, RoleId right) {
      return left.Equals(right);
    }

    public static bool operator !=(RoleId left, RoleId right) {
      return !left.Equals(right);
    }

    public bool Equals(RoleId other) {
      return _value.Equals(other._value);
    }

    public override bool Equals(object obj) {
      if (ReferenceEquals(null, obj)) return false;
      return obj is RoleId && Equals((RoleId)obj);
    }

    public override int GetHashCode() {
      return _value.GetHashCode();
    }

    public static implicit operator Guid(RoleId id) {
      return id._value;
    }

    public override string ToString() {
      return String.Format("Role/{0}", _value.ToString().ToUpperInvariant());
    }

    Guid IAggregateIdentity.Value { get { return _value; } }
  }
}