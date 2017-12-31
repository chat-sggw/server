using System;
using Neat.CQRSLite.Contract.Commands;

namespace ChatSggw.Domain.Commands.Conversation
{
    //todo [KR]: zweryfikowac, czy mozna zlaczyc z dodawaniem na tym etapie jako <<AddRemoveMember>>
    public class RemoveMemberFromConversationCommand : ICommand
    {
        public Guid ConversationId { get; set; }
        public Guid MemberId { get; set; }
    }
}
