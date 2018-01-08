using System;
using System.Data.Entity;
using System.Linq;
using ChatSggw.DataLayer;
using ChatSggw.Domain.Commands.Conversation;
using Neat.CQRSLite.Contract.Commands;

namespace ChatSggw.Services.Commands.Conversation
{
    public class RemoveMemberFromConversationCommandHandler : ICommandHandler<RemoveMemberFromConversationCommand>
    {
        private readonly CoreDbContext _db;

        public RemoveMemberFromConversationCommandHandler(CoreDbContext db)
        {
            _db = db;
        }

        public void Execute(RemoveMemberFromConversationCommand command)
        {            
            var conversation = _db.Conversations.Include(x => x.Members)
                .SingleOrDefault(x => x.Id == command.ConversationId);

            if (conversation == null)
            {
                throw new Exception("The conversation doesn't exist!");
            }

            if (conversation.Members.All(m => m.UserId != command.UserId))
            {
                throw new Exception("Brak uprawnień");
            }

            conversation.RemoveMember(command.MemberId);            
            _db.SaveChanges();
        }
    }
}