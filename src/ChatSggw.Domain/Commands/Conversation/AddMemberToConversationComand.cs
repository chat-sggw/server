using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatSggw.Domain.Commands.Conversation
{
    public class AddMemberToConversationComand
    {
        public Guid ConversationId { get; set; }
        public Guid MemberId { get; set; }
    }
}
