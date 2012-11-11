using System.Data;

namespace Seabites.Naughty.Infrastructure {
  public interface ISqlStatement {
    string Text { get; }
    object Parameters { get; }
    CommandType CommandType { get; }
  }
}