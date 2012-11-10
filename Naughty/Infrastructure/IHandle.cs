namespace Seabites.Naughty.Infrastructure {
  public interface IHandle<in TMessage> {
    void Handle(TMessage message);
  }
}