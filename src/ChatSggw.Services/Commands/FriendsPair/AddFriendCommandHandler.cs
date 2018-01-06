using ChatSggw.DataLayer;
using ChatSggw.Domain.Commands.FriendsPair;
using System;
using System.Linq;

namespace ChatSggw.Services.Commands.FriendsPair
{
    public class AddFriendCommandHandler
    {
        private readonly CoreDbContext _db;

        public AddFriendCommandHandler(CoreDbContext db)
        {
            _db = db;
        }

        public void Execute(AddRemoveFriendCommand command)
        {
            if (_db.FriendsPairs.Any(
                x => (x.FirstUserId == command.FirstUserId && x.SecondUserId == command.SecondUserId)
                || x.FirstUserId == command.SecondUserId && x.SecondUserId == command.FirstUserId))
            {
                throw new Exception("Those members are already friends!");
            }

            _db.FriendsPairs.Add(
                new Domain.Entities.FriendsPair.FriendsPair(command.FirstUserId, command.SecondUserId));
            _db.SaveChanges();
        }
    }
}
