using Neat.CQRSLite.Contract.Commands;

namespace ChatSggw.Domain.Commands.Message
{
    public class SendMessageCommand : ICommand
    {
        public Entities.Conversation.Message Message { get; set; }
    }
}
