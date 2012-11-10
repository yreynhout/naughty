using System;

namespace Seabites.Naughty.Security {
  public struct Name {
    readonly string _value;

    public Name(string value) {
      if(String.IsNullOrEmpty(value)) 
        throw new ArgumentException("A name cannot be null or empty.");
      if(value.Length > Metadata.NameMaxLength)
        throw new ArgumentException(string.Format("A name cannot be longer than {0} characters.", Metadata.NameMaxLength));
      _value = value;
    }

    public static implicit operator string(Name name) {
      return name._value;
    }
  }
}