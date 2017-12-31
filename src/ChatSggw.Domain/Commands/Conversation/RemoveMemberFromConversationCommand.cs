using System;

namespace ChatSggw.Domain.Commands.Conversation
{
    //todo [KR]: zweryfikowac, czy mozna zlaczyc z dodawaniem na tym etapie
    public class RemoveMemberFromConversationCommand
    {
        public Guid ConversationId { get; set; }
        public Guid MemberId { get; set; }
    }
}
