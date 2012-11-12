using System;

namespace Seabites.Naughty.Infrastructure {
  public class Repository<TAggregateRootEntity> : IRepository<TAggregateRootEntity>
    where TAggregateRootEntity : IAggregateRootEntity {
    readonly Func<Guid, TAggregateRootEntity> _factory;
    readonly IEventStreamReader _reader;
    readonly IUnitOfWork _unitOfWork;

    public Repository(Func<Guid, TAggregateRootEntity> factory, IEventStreamReader reader, IUnitOfWork unitOfWork) {
      if (factory == null) throw new ArgumentNullException("factory");
      if (reader == null) throw new ArgumentNullException("reader");
      if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
      _factory = factory;
      _reader = reader;
      _unitOfWork = unitOfWork;
    }

    public TAggregateRootEntity Get(Guid id) {
      IAggregateRootEntity weakRoot;
      if (_unitOfWork.TryGet(id, out weakRoot)) {
        return (TAggregateRootEntity) weakRoot;
      }
      var root = _factory(id);
      var stream = _reader.Read(id);
      root.Initialize(stream.Events);
      return root;
    }

    public void Add(TAggregateRootEntity root) {
      _unitOfWork.Attach(root);
    }
    }
}