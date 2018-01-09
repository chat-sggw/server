using ChatSggw.DataLayer;
using ChatSggw.Domain.Commands.FriendsPair;
using System;
using System.Linq;
using Neat.CQRSLite.Contract.Commands;

namespace ChatSggw.Services.Commands.FriendsPair
{
    public class AddFriendCommandHandler : ICommandHandler<AddFriendCommand>
    {
        private readonly CoreDbContext _db;

        public AddFriendCommandHandler(CoreDbContext db)
        {
            _db = db;
        }

        public void Execute(AddFriendCommand command)
        {
            if (_db.FriendsPairs.Any(
                x => (x.FirstUserId == command.UserId && x.SecondUserId == command.FriendId)
                     || x.FirstUserId == command.FriendId && x.SecondUserId == command.UserId))
            {
                //skip users are already friends
                return;
            }
            
            if (!_db.Users.Any(u => u.Id == command.FriendId))
            {
                throw new Exception("Używtkownik nie istnieje");
            }


            var conversation =
                Domain.Entities.Conversation.Conversation.CreateDirectConversation(command.UserId, command.FriendId);
            var pair = Domain.Entities.FriendsPair.FriendsPair.Create(command.UserId, command.FriendId,
                conversation.Id);
            _db.FriendsPairs.Add(pair);
            _db.Conversations.Add(conversation);
            _db.SaveChanges();
        }
    }
}