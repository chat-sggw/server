using Neat.CQRSLite.Contract.Commands;

namespace ChatSggw.Domain.Commands.Conversation
{
    public class CreateGroupConversationCommand : ICommand
    {
        public Entities.Conversation.Conversation Conversation { get; set; }
    }
}
