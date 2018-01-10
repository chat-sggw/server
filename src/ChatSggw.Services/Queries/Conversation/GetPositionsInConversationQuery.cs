using System;
using System.Collections.Generic;
using System.Linq;
using ChatSggw.DataLayer;
using ChatSggw.Domain.DTO.User;
using ChatSggw.Domain.Queries.Conversation;
using Neat.CQRSLite.Contract.Queries;

namespace ChatSggw.Services.Queries.Conversation
{
    public class
        GetPositionsInConversationQueryPerformer : IQueryPerformer<GetPositionsInConvesationQuery,
            IEnumerable<UserInfoPositionDTO>>
    {
        private readonly CoreDbContext _db;

        public GetPositionsInConversationQueryPerformer(CoreDbContext db)
        {
            _db = db;
        }

        public IEnumerable<UserInfoPositionDTO> Perform(GetPositionsInConvesationQuery query)
        {
            if (!_db.ConversationMembers.Any(cm =>
                cm.UserId == query.UserId && cm.ConversationId == query.ConversationId))
            {
                throw new Exception("Brak uprawnień");
            }

            return _db.ConversationMembers
                .Where(m => m.ConversationId == query.ConversationId)
                .ToList()
                .Join(_db.Users, m => m.UserId, u => u.Id, (m, u) => new UserInfoPositionDTO()
                {
                    UserId = u.Id,
                    UserName = u.UserName,
                    GeoStamp = u.CurrentPosition
                })
                .ToList();            
        }
    }
}