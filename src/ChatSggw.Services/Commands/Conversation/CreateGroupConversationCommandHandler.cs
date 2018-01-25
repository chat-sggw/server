using System.Linq;
using ChatSggw.DataLayer;
using ChatSggw.Domain.Commands.Conversation;
using Neat.CQRSLite.Contract.Commands;

namespace ChatSggw.Services.Commands.Conversation
{
    public class CreateGroupConversationCommandHandler : ICommandHandler<CreateGroupConversationCommand>
    {
        private readonly CoreDbContext _db;

        public CreateGroupConversationCommandHandler(CoreDbContext db)
        {
            _db = db;
        }

        public void Execute(CreateGroupConversationCommand command)
        {
            var members = command.Members.ToList();
            members.Add(command.UserId);

            var conversation =
                Domain.Entities.Conversation.Conversation.CreateGroupConversation(command.NewId, members,
                    command.IsGeoConversation);
            
            _db.Conversations.Add(conversation);
            _db.SaveChanges();
        }
    }
}