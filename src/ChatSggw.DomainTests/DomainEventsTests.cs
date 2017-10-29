using System;
using ChatSggw.Domain;
using Neat.CQRSLite.Contract.Events;
using Xunit;

namespace ChatSggw.DomainTests
{
    public class DomainEventsTests
    {
        public class TestEvent : IEvent
        {
        }

        public class SubTestEvent : TestEvent
        {
        }


        public class AnotherTestEvent : IEvent
        {
        }

        public class TestEventBuss : IEventBus
        {
            private readonly Action<Type, IEvent> _handler;

            public TestEventBuss(Action<Type, IEvent> handler)
            {
                _handler = handler;
            }

            public void Send<T>(T @event) where T : IEvent
            {
                _handler.Invoke(typeof(T),@event);
            }
        }

        [Fact]
        public void CheckIfClearEventsWorks()
        {
            //Arrange
            var eventHandlerCallCount = 0;
            DomainEvents.Register<TestEvent>(e => { eventHandlerCallCount++; });

            //Act
            DomainEvents.ClearCallbacks();
            DomainEvents.Raise(new TestEvent()); // event is published after using statement 

            //Assert
            Assert.Equal(0, eventHandlerCallCount);
        }

        [Fact]
        public void TestThatDomainEventsRaiseRightGenericHandlers()
        {
            //Arrange
            var testEventHandlerCallCount = 0;
            var anotherEventHandlerCallCount = 0;
            DomainEvents.Register<TestEvent>(e => { testEventHandlerCallCount++; });
            DomainEvents.Register<AnotherTestEvent>(e => { anotherEventHandlerCallCount++; });

            //Act
            DomainEvents.Raise(new TestEvent());

            //Assert
            Assert.Equal(1, testEventHandlerCallCount);
            Assert.Equal(0, anotherEventHandlerCallCount);
        }

        [Fact]
        public void TestThatDomainEventsWorkWithMultipleHandlers()
        {
            //Arrange
            var firstEventHandlerCallCount = 0;
            var secondEventHandlerCallCount = 0;
            DomainEvents.Register<TestEvent>(e => { firstEventHandlerCallCount++; });
            DomainEvents.Register<TestEvent>(e => { secondEventHandlerCallCount++; });

            //Act
            DomainEvents.Raise(new TestEvent());

            //Assert
            Assert.Equal(1, firstEventHandlerCallCount);
            Assert.Equal(1, secondEventHandlerCallCount);
        }

        [Fact]
        public void TestThatDomainEventsWorkWithSubClasses()
        {
            //Arrange
            var testEventHandlerCallCount = 0;
            var subTestEventHandlerCallCount = 0;
            DomainEvents.Register<TestEvent>(e => testEventHandlerCallCount++);
            DomainEvents.Register<SubTestEvent>(e => subTestEventHandlerCallCount++);

            //Act
            DomainEvents.Raise(new SubTestEvent());

            //Assert
            Assert.Equal(1, testEventHandlerCallCount);
            Assert.Equal(1, subTestEventHandlerCallCount);
        }

        [Fact]
        public void TestThatDomainEventsWorkWithEventBuss()
        {
            //Arrange
            var eventBusCallCount = 0;
            DomainEvents.EventBus = new TestEventBuss((t, e) => eventBusCallCount++);

            //Act
            DomainEvents.Raise(new TestEvent());

            //Assert
            Assert.Equal(1, eventBusCallCount);
        }

        [Fact]
        public void TestThatEventIsPublished()
        {
            //Arrange
            var eventHandlerCallCount = 0;
            DomainEvents.Register<TestEvent>(e => { eventHandlerCallCount++; });

            //Act
            DomainEvents.Raise(new TestEvent());

            //Assert
            Assert.Equal(1, eventHandlerCallCount);
        }
    }
}