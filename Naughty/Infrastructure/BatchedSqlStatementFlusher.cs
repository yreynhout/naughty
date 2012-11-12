using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Seabites.Naughty.Infrastructure {
  public class BatchedSqlStatementFlusher {
    readonly SqlConnectionStringBuilder _builder;

    public BatchedSqlStatementFlusher(SqlConnectionStringBuilder builder) {
      if (builder == null) throw new ArgumentNullException("builder");
      _builder = builder;
    }

    public void Flush(IEnumerable<ISqlStatement> statements) {
      var writer = new SqlStatementBatchWriter();
      using (var connection = new SqlConnection(_builder.ConnectionString)) {
        connection.Open();
        using (var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted)) {
          using (var command = new SqlCommand()) {
            command.Connection = connection;
            command.Transaction = transaction;
            using (var enumerator = statements.GetEnumerator()) {
              var moved = enumerator.MoveNext();
              while (moved) {
                while (moved && writer.TryWrite(enumerator.Current)) {
                  moved = enumerator.MoveNext();
                }
                writer.BuildCommand(command);
                command.ExecuteNonQuery();
                writer.Reset();
              }
            }
          }
          transaction.Commit();
        }
        connection.Close();
      }
    }
  }
}