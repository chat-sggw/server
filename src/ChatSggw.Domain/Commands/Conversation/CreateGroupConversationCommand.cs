using System;
using Neat.CQRSLite.Contract.Commands;

namespace ChatSggw.Domain.Commands.Conversation
{
    public class CreateGroupConversationCommand : ICommand
    {
        public Guid[] Members { get; set; }
        public Guid UserId { get; set; }
    }
}
