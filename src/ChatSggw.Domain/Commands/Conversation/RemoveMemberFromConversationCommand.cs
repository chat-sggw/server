using System;
using Neat.CQRSLite.Contract.Commands;

namespace ChatSggw.Domain.Commands.Conversation
{
    public class RemoveMemberFromConversationCommand : ICommand
    {
        public Guid ConversationId { get; set; }
        public Guid MemberId { get; set; }
        public Guid UserId { get; set; }
    }
}
