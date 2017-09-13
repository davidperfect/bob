using System.IO;
using System.Threading.Tasks;

namespace OrderBookLib.EventStorage
{
    class InputEventSteam<TEvent> : IInputEventSteam<TEvent>
    {
        string _path;
        private StreamReader _streamReader;
        private IMessageSerializer<TEvent> _serializer;

        public InputEventSteam(string path, IMessageSerializer<TEvent> serializer)
        {
            _path = path;
            var fileStream = File.OpenRead(_path);
            _streamReader = new StreamReader(fileStream);
            _serializer = serializer;
        }

        public async Task<TEvent> ReadEventAsync()
        {
            var line = await _streamReader.ReadLineAsync();
            var anEvent = _serializer.Deserialize(line);
            return anEvent;
        }
    }
}
