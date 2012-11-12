using System.Data;

namespace Seabites.Naughty.Infrastructure {
  public class SqlStoredProcedureStatement : ISqlStatement {
    readonly string _text;
    readonly object _parameters;

    public SqlStoredProcedureStatement(string text, object parameters) {
      _text = text;
      _parameters = parameters;
    }

    public string Text {
      get { return _text; }
    }

    public object Parameters {
      get { return _parameters; }
    }

    public CommandType CommandType {
      get { return CommandType.StoredProcedure; }
    }
  }
}