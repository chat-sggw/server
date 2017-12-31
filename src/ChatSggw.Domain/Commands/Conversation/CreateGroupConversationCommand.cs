using ChatSggw.Domain.Entities.Conversation;
using Neat.CQRSLite.Contract.Commands;

namespace ChatSggw.Domain.Commands.User
{
    public class CreateGroupConversationCommand : ICommand
    {
        public Conversation NewConversation { get; set; }
    }
}
