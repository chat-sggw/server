using System;
using System.Linq;
using ChatSggw.DataLayer;
using ChatSggw.Domain.Commands.FriendsPair;
using ChatSggw.Domain.Commands.User;
using Neat.CQRSLite.Contract.Commands;

namespace ChatSggw.Services.Commands.FriendsPair
{
    public class RemoveFriendCommandHandler : ICommandHandler<RemoveFriendCommand>
    {
        private readonly CoreDbContext _db;

        public RemoveFriendCommandHandler(CoreDbContext db)
        {
            _db = db;
        }

        public void Execute(RemoveFriendCommand command)
        {
            var friendsPair = _db.FriendsPairs.SingleOrDefault(
                x => (x.FirstUserId == command.UserId && x.SecondUserId == command.FriendId)
                     || x.FirstUserId == command.FriendId && x.SecondUserId == command.UserId);
            if (friendsPair == null)
            {
                //skip they are no longer friends 
                return;
            }

            _db.FriendsPairs.Remove(friendsPair);
            _db.SaveChanges();
        }
    }
}
