using System;
using ChatSggw.DataLayer;
using ChatSggw.Domain.Commands.FriendsPair;
using ChatSggw.Domain.Commands.User;

namespace ChatSggw.Services.Commands.FriendsPair
{
    public class RemoveFriendCommandHandler
    {
        private readonly CoreDbContext _db;

        public RemoveFriendCommandHandler(CoreDbContext db)
        {
            _db = db;
        }

        public void Execute(AddRemoveFriendCommand command)
        {
            var friendsPair = _db.FriendsPairs.Find(command.FirstUserId, command.SecondUserId);

            if (friendsPair == null)
            {
                throw new Exception("Those members are not friends!");
            }

            _db.FriendsPairs.Remove(friendsPair);
            _db.SaveChanges();
        }
    }
}
