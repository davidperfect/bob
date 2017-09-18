using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrderBookLib.EventStorage;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace OrderBookLib.IntegrationTests
{
    [TestClass]
    public class EventStoreTests
    {

        public class TestEvent
        {
            public string TestProperty { get; set; }
        }

        public class TestSubscriber : IEventStreamSubscriber<TestEvent>
        {
            public string TestProperty;
            public Task HandleEventAsync(TestEvent anEvent)
            {
                TestProperty = anEvent.TestProperty;
                return Task.CompletedTask;
            }
        }

        [TestMethod]
        public async Task TestMethod1()
        {
            var serializer = new JsonMessageSerializer<TestEvent>();
            File.Delete("TestMethod1.txt");
            var store = new EventStore<TestEvent>("TestMethod1.txt", serializer);
            var anEvent = new TestEvent
            {
                TestProperty = "hello"
            };
            await store.WriteEventAsync(anEvent);
            var testSubscriber = new TestSubscriber();
            CancellationTokenSource cts = new CancellationTokenSource();
            var readTask = store.ReadAsync(testSubscriber, cts.Token);
            await Task.Delay(1000);
            cts.Cancel();
            await readTask;
            Assert.AreEqual("hello", testSubscriber.TestProperty);
        }

        [TestMethod]
        public async Task TestMethod2()
        {
            var serializer = new JsonMessageSerializer<TestEvent>();
            File.Delete("TestMethod2.txt");
            var store = new EventStore<TestEvent>("TestMethod2.txt", serializer);
            var anEvent = new TestEvent
            {
                TestProperty = "hello"
            };
            var testSubscriber = new TestSubscriber();
            CancellationTokenSource cts = new CancellationTokenSource();
            var readTask = store.ReadAsync(testSubscriber, cts.Token);
            await Task.Delay(100);
            await store.WriteEventAsync(anEvent);
            await Task.Delay(1000);
            cts.Cancel();
            await readTask;
            Assert.AreEqual("hello", testSubscriber.TestProperty);
        }
    }
}
