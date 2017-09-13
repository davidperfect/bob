using System.IO;

namespace OrderBookLib.EventStorage
{
    interface IMessageSerializer<T>
    {
        void Serialize(TextWriter textWriter, T message);

        T Deserialize(string message);
    }
}
