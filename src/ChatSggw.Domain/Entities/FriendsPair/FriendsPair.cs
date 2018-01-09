using System;

namespace ChatSggw.Domain.Entities.FriendsPair
{
    public class FriendsPair
    {
        public Guid FirstUserId { get; set; }
        public Guid SecondUserId { get; set; }
        public Guid ConversationId { get; set; }

        public static FriendsPair Create(Guid firstUserId, Guid secondUserId, Guid conversationId)
        {
            return new FriendsPair
            {
                FirstUserId = firstUserId,
                SecondUserId = secondUserId,
                ConversationId = conversationId,
            };
        }
    }
}