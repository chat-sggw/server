using System;

namespace ChatSggw.Domain.Entities.User
{
    public class UserDirectConversation
    {
        public Guid UserId { get; set; }
        public Guid InterlocutorId { get; set; }
        public Guid ConversationId { get; set; }
        public bool IsFriend { get; set; }

    }
}