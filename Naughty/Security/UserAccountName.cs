using System;

namespace Seabites.Naughty.Security {
  public struct UserAccountName {
    readonly string _value;

    public UserAccountName(string value) {
      if (String.IsNullOrEmpty(value))
        throw new ArgumentException("A user account name cannot be null or empty.");
      if (value.Length > Metadata.UserAccountNameMaxLength)
        throw new ArgumentException(string.Format("A user account name cannot be longer than {0} characters.", Metadata.UserAccountNameMaxLength));
      _value = value;
    }

    public static implicit operator string(UserAccountName name) {
      return name._value;
    }
  }
}