using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace OrderBookLib.EventStorage
{
    class InputEventSteam<TEvent>
    {
        string _path;
        private StreamReader _streamReader;
        private IMessageSerializer<TEvent> _serializer;

        public InputEventSteam(string path, IMessageSerializer<TEvent> serializer)
        {
            _path = path;
            var fileStream = File.Open(_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            _streamReader = new StreamReader(fileStream);
            _serializer = serializer;
        }

        public async Task<TEvent> ReadEventAsync(CancellationToken ct)
        {
            string line = null;
            while (!ct.IsCancellationRequested)
            {
                line = await _streamReader.ReadLineAsync();
                if (line != null)
                {
                    var anEvent = _serializer.Deserialize(line);
                    return anEvent;
                }

                await Task.Delay(100);
            }

            return default(TEvent);
        }
    }
}
