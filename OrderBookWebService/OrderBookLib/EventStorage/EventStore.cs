using System.IO;
using System.Threading.Tasks;

namespace OrderBookLib.EventStorage
{
    class EventStore<TEvent> : IOutputEventSteam<TEvent>
    {
        string _path;
        private StreamWriter _streamWriter;
        private IMessageSerializer<TEvent> _serializer;

        public EventStore(string path, IMessageSerializer<TEvent> serializer)
        {
            _path = path;
            var fileStream = File.OpenWrite(_path);
            _streamWriter = new StreamWriter(fileStream);
            _serializer = serializer;
        }

        public Task WriteEventAsync(TEvent anEvent)
        {
            _serializer.Serialize(_streamWriter, anEvent);
            return _streamWriter.WriteLineAsync();
        }

        public Task<IInputEventSteam<TEvent>> StartReadAsync(TEvent anEvent)
        {
            IInputEventSteam<TEvent> result = new InputEventSteam<TEvent>(_path, _serializer);
            return Task.FromResult(result);
        }
    }
}
