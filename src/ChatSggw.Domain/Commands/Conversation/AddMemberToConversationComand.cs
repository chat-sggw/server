using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neat.CQRSLite.Contract.Commands;

namespace ChatSggw.Domain.Commands.Conversation
{
    public class AddMemberToConversationComand : ICommand
    {
        public Guid ConversationId { get; set; }
        public Guid MemberId { get; set; }
    }
}
