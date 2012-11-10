using System;
using System.Collections.Generic;

namespace Seabites.Naughty.Infrastructure {
  public abstract class AggregateRootEntity<TIdentity> : IAggregateRootEntity where TIdentity : IAggregateIdentity {
    public const int InitialVersion = 0;

    int _version;
    int _baseVersion;

    object[] _changes;
    int _size;

    readonly Dictionary<Type, Action<object>> _applyMethods;
    readonly TIdentity _id;

    protected AggregateRootEntity(TIdentity id) {
      _id = id;
      _version = InitialVersion;
      _baseVersion = InitialVersion;

      _changes = new object[2];
      _size = 0;

      _applyMethods = new Dictionary<Type, Action<object>>();
    }

    public TIdentity Id {
      get { return _id; }
    }

    Guid IAggregateRootEntity.Id {
      get { return _id.Value; }
    }

    public int Version {
      get { return _version; }
    }

    public int BaseVersion {
      get { return _baseVersion; }
    }

    protected void Register<TEvent>(Action<TEvent> method) {
      _applyMethods.Add(typeof (TEvent), @event => method((TEvent) @event));
    }

    public override bool Equals(object obj) {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != GetType()) return false;
      var other = (AggregateRootEntity<TIdentity>) obj;
      return _id.Equals(other._id);
    }

    public override int GetHashCode() {
      return _id.GetHashCode();
    }

    public void Initialize(IEnumerable<object> events) {
      _version = 0;
      foreach(var @event in events) {
        Play(@event);
        _version++;
      }
      _baseVersion = _version;
    }

    protected void ApplyEvent(object @event) {
      Play(@event);
      Record(@event);
      _version++;
    }

    void Play(object @event) {
      Action<object> method;
      if(_applyMethods.TryGetValue(@event.GetType(), out method)) {
        method(@event);
      }
    }

    void Record(object @event) {
      if(_size == _changes.Length) {
        var copy = new object[_size*2];
        Array.Copy(_changes, 0, copy, 0, _size);
        _changes = copy;
      }
      _changes[_size++] = @event;
    }

    public bool HasChanges() {
      return _size != 0;
    }

    public IEnumerable<object> GetChanges() {
      var copy = new object[_size];
      Array.Copy(_changes, 0, copy, 0, _size);
      return copy;
    }

    public void ClearChanges() {
      _changes = new object[2];
      _size = 0;
      _baseVersion = _version;
    }
  }
}