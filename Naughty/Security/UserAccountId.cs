using System;
using Seabites.Naughty.Infrastructure;

namespace Seabites.Naughty.Security {
  public struct UserAccountId : IEquatable<UserAccountId>, IAggregateIdentity {
    readonly Guid _value;

    public UserAccountId(Guid value) {
      _value = value;
    }

    public static bool operator ==(UserAccountId left, UserAccountId right) {
      return left.Equals(right);
    }

    public static bool operator !=(UserAccountId left, UserAccountId right) {
      return !left.Equals(right);
    }

    public bool Equals(UserAccountId other) {
      return _value.Equals(other._value);
    }

    public override bool Equals(object obj) {
      if (ReferenceEquals(null, obj)) return false;
      return obj is UserAccountId && Equals((UserAccountId)obj);
    }

    public override int GetHashCode() {
      return _value.GetHashCode();
    }

    public static implicit operator Guid(UserAccountId id) {
      return id._value;
    }

    public override string ToString() {
      return String.Format("UserAccount/{0}", _value.ToString().ToUpperInvariant());
    }

    Guid IAggregateIdentity.Value { get { return _value; } }
  }
}