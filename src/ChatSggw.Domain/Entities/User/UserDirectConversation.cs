using System;
using Neat.CQRSLite.Contract.Domain;

namespace ChatSggw.Domain.Entities.User
{
    public class UserDirectConversation : IAggregate
    {
        public Guid Id { get; set; }
        public Guid ConversationId { get; set; }
        public Guid UserId { get; set; }
        public Guid InterlocutorId { get; set; }
        public bool IsFriend { get; set; }

    }
}