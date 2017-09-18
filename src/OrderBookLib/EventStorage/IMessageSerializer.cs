using System.IO;

namespace OrderBookLib.EventStorage
{
    public interface IMessageSerializer<T>
    {
        void Serialize(TextWriter textWriter, T message);

        T Deserialize(string message);
    }
}
