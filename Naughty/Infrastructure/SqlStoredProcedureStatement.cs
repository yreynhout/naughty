using System.IO;

namespace Seabites.Naughty.Infrastructure {
  public class SqlStoredProcedureStatement : ISqlStatement {
    readonly string _text;
    readonly object _parameters;

    public SqlStoredProcedureStatement(string text, object parameters) {
      _text = text;
      _parameters = parameters;
    }

    public void WriteSql(TextWriter writer) {
      writer.WriteLine(_text);
    }
  }
}