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

            _db.FriendsPairs.Add(
                new Domain.Entities.FriendsPair.FriendsPair(command.UserId, command.FriendId));
            _db.SaveChanges();
        }
    }
}