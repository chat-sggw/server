using System.Collections.Generic;
using System.Linq;
using ChatSggw.DataLayer;
using ChatSggw.Domain.DTO.User;
using ChatSggw.Domain.Queries.User;
using Neat.CQRSLite.Contract.Queries;

namespace ChatSggw.Services.Queries.User
{
    public class GetMyFriendsInfoQueryPerformer : IQueryPerformer<GetMyFriendsInfoQuery, IEnumerable<FriendInfoDTO>>
    {
        private readonly CoreDbContext _db;

        public GetMyFriendsInfoQueryPerformer(CoreDbContext db)
        {
            _db = db;
        }

        public IEnumerable<FriendInfoDTO> Perform(GetMyFriendsInfoQuery query)
        {
            return _db.FriendsPairs
                .Where(pair => pair.FirstUserId == query.UserId || pair.SecondUserId == query.UserId)
                .Select(pair => new
                {
                    pair.ConversationId,
                    FriendId = pair.FirstUserId != query.UserId ? pair.FirstUserId : pair.SecondUserId
                })
                .Join(_db.Users, pair => pair.FriendId, user => user.Id, (pair, user) => new FriendInfoDTO
                {
                    ConversationId = pair.ConversationId,
                    FriendId = user.Id,
                    UserName = user.UserName,
                    HasUnreadMessages = false, //todo
                    IsActive = true //todo
                })
                .ToList();
        }
    }
}