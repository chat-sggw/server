using ChatSggw.DataLayer;
using ChatSggw.Domain.Commands.User;
using ChatSggw.Domain.Entities.FriendsPair;

namespace ChatSggw.Services.Commands.User
{
    public class AddFriendCommandHandler
    {
        private readonly CoreDbContext _db;

        public AddFriendCommandHandler(CoreDbContext db)
        {
            _db = db;
        }

        public void Execute(AddFriendCommand command)
        {
            _db.FriendsPairs.Add(
                new FriendsPair(command.FirstUserId, command.SecondUserId));
            _db.SaveChanges();
        }
    }
}
