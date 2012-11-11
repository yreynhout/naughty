using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Seabites.Naughty.Infrastructure {
  public class SqlStatementBatchWriter {
    readonly List<SqlParameter> _parameters;
    string _text;
    short _parameterIndex;

    public SqlStatementBatchWriter() {
      _text = "";
      _parameterIndex = 0;
      _parameters = new List<SqlParameter>();
    }

    public bool TryWrite(ISqlStatement statement) {
      // Bad code ... you're assuming order!
      var properties = statement.Parameters.GetType().GetProperties().ToArray();
      if((_parameters.Count + properties.Length) <= 2100) {
        if(statement is SqlStoredProcedureStatement) {
          var text = statement.Text;
          var index = 0;
          foreach (var property in properties) {
            var parameterName = "@P" + _parameterIndex++;
            _parameters.Add(
              new SqlParameter(parameterName, property.GetValue(statement.Parameters)));
            if(index > 0) {
              text = text + ", " + parameterName;
            } else {
              text = text + " " + parameterName;
            }
            index++;
          }
          _text = _text + "EXEC " + text + ";";
        } else if(statement is SqlTextStatement) {
          var text = statement.Text;
          foreach (var property in properties) {
            var parameterName = "@P" + _parameterIndex++;
            _parameters.Add(
              new SqlParameter(parameterName, property.GetValue(statement.Parameters)));
            text = text.Replace("@" + property.Name, parameterName);
          }
          _text = _text + text + ";";  
        }
        
        return true;
      }
      return false;
    }

    public void BuildCommand(SqlCommand command) {
      command.CommandText = _text;
      command.Parameters.Clear();
      foreach (var parameter in _parameters) {
        command.Parameters.Add(parameter);
      }
    }

    public void Reset() {
      _text = "";
      _parameterIndex = 0;
      _parameters.Clear();
    }
  }
}
