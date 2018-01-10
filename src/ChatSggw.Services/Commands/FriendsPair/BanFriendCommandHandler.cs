using ChatSggw.DataLayer;
using ChatSggw.Domain.Commands.FriendsPair;
using System;
using System.Linq;
using Neat.CQRSLite.Contract.Commands;

namespace ChatSggw.Services.Commands.FriendsPair
{
    public class BanFriendCommandHandler : ICommandHandler<BanFriendCommand>
    {
        private readonly CoreDbContext _db;

        public BanFriendCommandHandler(CoreDbContext db)
        {
            _db = db;
        }

        public void Execute(BanFriendCommand command)
        {
            // Sprawdzam, czy istnieje powiązanie między osobami w tabeli BannedFriendsPairs.
            if (_db.BannedPairs.Any(
                x => (x.FirstUserId == command.UserId && x.SecondUserId == command.FriendId)))
            {
                // Pomijam, użytkownik jest już zablokowany.
                return;
            }
            
            // Sprawdzam, czy użytkownik do zablokowania istnieje w bazie.
            if (!_db.Users.Any(u => u.Id == command.FriendId))
            {
                throw new Exception("Użytkownik, którego chcesz zablokować nie istnieje.");
            }

            // Sprawdzam, czy w tabeli FriendsPairs istnieje powiązanie między osobami
            var friendsPair = _db.FriendsPairs.SingleOrDefault(
                x => (x.FirstUserId == command.UserId && x.SecondUserId == command.FriendId)
                     || x.FirstUserId == command.FriendId && x.SecondUserId == command.UserId);

            if (friendsPair == null)
            {
                // Pomijam, osoby nie są znajomymi
                return;
            }

            var conversation = _db.Conversations.Find(friendsPair.ConversationId);
            var bannedPair = Domain.Entities.FriendsPair.BannedPair.Create(command.UserId, command.FriendId, friendsPair.ConversationId);

            _db.BannedPairs.Add(bannedPair);
            _db.FriendsPairs.Remove(friendsPair);
            _db.Conversations.Remove(conversation);
            _db.SaveChanges();
        }
    }
}