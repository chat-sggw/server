using System;

namespace ChatSggw.Domain.Entities.BannedPair
{
    public class BannedPair
    {
        public Guid FirstUserId { get; set; }
        public Guid SecondUserId { get; set; }
        public Guid ConversationId { get; set; }

        public static BannedPair Create(Guid firstUserId, Guid secondUserId, Guid conversationId)
        {
            return new BannedPair
            {
                FirstUserId = firstUserId,
                SecondUserId = secondUserId,
                ConversationId = conversationId,
            };
        }
    }
}