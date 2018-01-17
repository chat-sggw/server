using System;

namespace ChatSggw.Domain.Entities.Conversation
{
    public class ConversationMember
    {
        public Guid ConversationId { get; private set; }
        public Guid UserId { get; private set; }
        public double GeoConversationRange { get; private set; }

        public static ConversationMember Create(Guid conversationId, Guid userId, double range = 0)
        {
            return new ConversationMember
            {
                ConversationId = conversationId,
                UserId = userId,
                GeoConversationRange = range,
            };
        }

        /// <summary>
        /// Metoda ustawia wartość maksymalnego zasięgu geo-konwersacji
        /// </summary>
        /// <param name="range"> Zasięg konwersacji wyrażony w kilometrach.</param>
        /// <exception cref="ArgumentException"></exception>
        public void SetConversationRange(double range)
        {
            if (range < 0)
            {
                throw new ArgumentException("Geo-conversation range must be greater than 0!");
            }
            GeoConversationRange = range;
        }
    }
}