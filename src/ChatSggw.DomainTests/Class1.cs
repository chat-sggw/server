using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatSggw.Domain;
using Neat.CQRSLite.Contract.Events;
using Xunit;

namespace ChatSggw.DomainTests
{
    public class DomainEventsTests
    {
        [Fact]
        public void TestThatEventIsPublished()
        {
            const string eventName = "Test1";
            var isEventPublished = false;
            DomainEvents.RegisterEventHandler(e => isEventPublished = (e as TestEvent)?.Name == eventName );
            DomainEvents.Publish(new TestEvent(eventName));

            Assert.True(isEventPublished);
        }

        [Fact]
        public void TestThatDomainEventsWorkWithMultipleHandlers()
        {
            const string eventName = "Test1";
            var isEventReceiveByFirstHandler = false;
            var isEventReceiveBySecondHandler = false;
            DomainEvents.RegisterEventHandler(e => isEventReceiveByFirstHandler = (e as TestEvent)?.Name == eventName);
            DomainEvents.RegisterEventHandler(e => isEventReceiveBySecondHandler = (e as TestEvent)?.Name == eventName);
            DomainEvents.Publish(new TestEvent(eventName));

            Assert.True(isEventReceiveByFirstHandler);
            Assert.True(isEventReceiveBySecondHandler);
        }

        [Fact]
        public void CheckIfCancelationTokensWorks()
        {
            const string eventName = "Test1";
            var isEventReceived = false;
            using (var token = DomainEvents.RegisterEventHandler(e => isEventReceived = (e as TestEvent)?.Name == eventName))
            {
                
            }
            DomainEvents.Publish(new TestEvent(eventName)); // event is published after using statsment 

            Assert.False(isEventReceived);
        }


        public class TestEvent : IEvent
        {
            public TestEvent()
            {
            }

            public TestEvent(string name)
            {
                Name = name;
            }


            public string Name { get; set; }
        }
    }
}