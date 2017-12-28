using System;

namespace ChatSggw.Domain.Entities.Conversation
{
    public class ConversationMember
    {
        public Guid ConversationId { get; private set; }
        public Guid UserId { get; private set; }

        public static ConversationMember Create(Guid conversationId, Guid userId)
        {
            return new ConversationMember
            {
                ConversationId = conversationId,
                UserId = userId,
            };
        }
    }
}