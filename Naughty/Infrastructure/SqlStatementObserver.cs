using System;
using System.Collections.Generic;

namespace Seabites.Naughty.Infrastructure {
  public class SqlStatementObserver : IObserver<ISqlStatement> {
    readonly List<ISqlStatement> _statements;

    public SqlStatementObserver() {
      _statements = new List<ISqlStatement>();
    }

    public ISqlStatement[] Statements {
      get { return _statements.ToArray(); }
    }

    public void OnNext(ISqlStatement value) {
      _statements.Add(value);
    }

    public void OnError(Exception error) {}

    public void OnCompleted() {}
  }
}