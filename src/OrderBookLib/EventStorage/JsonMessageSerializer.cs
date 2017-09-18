using Newtonsoft.Json;
using System.IO;

namespace OrderBookLib.EventStorage
{
    public class JsonMessageSerializer<T> : IMessageSerializer<T>
    {
        JsonSerializer _jsonSerializer;

        public JsonMessageSerializer()
        {
            _jsonSerializer = new JsonSerializer();
        }

        public void Serialize(TextWriter tw, T message)
        {
            _jsonSerializer.Serialize(tw, message);
        }

        public T Deserialize(string message)
        {
            return JsonConvert.DeserializeObject<T>(message);
        }
    }
}
