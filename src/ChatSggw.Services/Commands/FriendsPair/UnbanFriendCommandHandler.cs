using ChatSggw.DataLayer;
using System;
using System.Linq;
using Neat.CQRSLite.Contract.Commands;
using ChatSggw.Domain.Commands.BannedPair;

namespace ChatSggw.Services.Commands.FriendsPair
{
    public class UnbanFriendCommandHandler : ICommandHandler<UnbanFriendCommand>
    {
        private readonly CoreDbContext _db;

        public UnbanFriendCommandHandler(CoreDbContext db)
        {
            _db = db;
        }

        public void Execute(UnbanFriendCommand command)
        {
            // Sprawdzam, czy użytkownik do odbanowania istnieje w bazie.
            if (!_db.Users.Any(u => u.Id == command.FriendId))
            {
                throw new Exception("Użytkownik, którego chcesz odblokować nie istnieje.");
            }

            // Sprawdzam, czy osoba była zbanowana
            var bannedPair = _db.BannedPairs.SingleOrDefault(
                x => (x.FirstUserId == command.UserId && x.SecondUserId == command.FriendId));

            if (bannedPair == null)
            {
                // Pomijam, osoba nie jest zbanowana
                return;
            }

            _db.BannedPairs.Remove(bannedPair);
            _db.SaveChanges();
        }
    }
}