using System.IO;

namespace Seabites.Naughty.Infrastructure {
  public interface ISqlStatement {
    void WriteSql(TextWriter writer);
  }
}