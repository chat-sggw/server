using System;
using System.Data.Entity;
using System.Linq;
using ChatSggw.DataLayer;
using ChatSggw.Domain;
using ChatSggw.Domain.Commands.Message;
using Neat.CQRSLite.Contract.Commands;

namespace ChatSggw.Services.Commands.Message
{
    public class SendMessageCommandHandler : ICommandHandler<SendMessageCommand>
    {
        private readonly CoreDbContext _db;

        public SendMessageCommandHandler(CoreDbContext db)
        {
            _db = db;
        }

        public void Execute(SendMessageCommand command)
        {            

            var user = _db.Users.Find(command.UserId);
            var conversation = _db.Conversations.Include(x => x.Members)
                .SingleOrDefault(x => x.Id == command.ConversationId);

            if (conversation == null)
            {
                throw new Exception("The conversation doesn't exist!");
            }

            if (conversation.Members.All(m => m.UserId != user.Id))
            {
                throw new Exception("Brak uprawnień");
            }

            conversation.AddMessage(command.Text, user.Id, user.CurrentPosition);
            _db.SaveChanges();
        }
    }
}