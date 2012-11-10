using System;

namespace Seabites.Naughty.Security {
  public struct PermissionId : IEquatable<PermissionId> {
    public static readonly PermissionId None = new PermissionId(Guid.Empty);

    public static bool operator ==(PermissionId left, PermissionId right) {
      return left.Equals(right);
    }

    public static bool operator !=(PermissionId left, PermissionId right) {
      return !left.Equals(right);
    }

    public bool Equals(PermissionId other) {
      return _value.Equals(other._value);
    }

    public override bool Equals(object obj) {
      if (ReferenceEquals(null, obj)) return false;
      return obj is PermissionId && Equals((PermissionId) obj);
    }

    public override int GetHashCode() {
      return _value.GetHashCode();
    }

    readonly Guid _value;

    public PermissionId(Guid value) {
      _value = value;
    }

    public static implicit operator Guid(PermissionId id) {
      return id._value;
    }

    public override string ToString() {
      return String.Format("Permission/{0}", _value.ToString().ToUpperInvariant());
    }
  }
}