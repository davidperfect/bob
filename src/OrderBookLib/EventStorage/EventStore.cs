using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace OrderBookLib.EventStorage
{
    public class EventStore<TEvent> : IOutputEventSteam<TEvent>
    {
        string _path;
        private StreamWriter _streamWriter;
        private IMessageSerializer<TEvent> _serializer;

        public EventStore(string path, IMessageSerializer<TEvent> serializer)
        {
            _path = path;
            var fileStream = File.Open(_path, FileMode.Append, FileAccess.Write, FileShare.Read);
            _streamWriter = new StreamWriter(fileStream);
            _serializer = serializer;
        }

        public async Task WriteEventAsync(TEvent anEvent)
        {
            _serializer.Serialize(_streamWriter, anEvent);
            await _streamWriter.WriteLineAsync();
            await _streamWriter.FlushAsync();
        }

        public Task ReadAsync(IEventStreamSubscriber<TEvent> subscriber, CancellationToken ct)
        {
            var workerTask = new Task(() => ProcessMessages(subscriber, ct), TaskCreationOptions.LongRunning);
            workerTask.Start();
            return workerTask;
        }

        private void ProcessMessages(IEventStreamSubscriber<TEvent> subscriber, CancellationToken ct)
        {
            var eventStream = new InputEventSteam<TEvent>(_path, _serializer);

            try
            {
                while (!ct.IsCancellationRequested)
                {
                    var anEvent = eventStream.ReadEventAsync(ct).Result;
                    if (anEvent != null)
                    {
                        subscriber.HandleEventAsync(anEvent).Wait();
                    }
                }
            }
            catch (System.Exception)
            {

                throw;
            }

        }
    }
}
