using System.Collections.Generic;
using System.Linq;
using ChatSggw.DataLayer;
using ChatSggw.Domain.DTO.User;
using ChatSggw.Domain.Queries.Conversation;
using Neat.CQRSLite.Contract.Queries;

namespace ChatSggw.Services.Queries.Conversation
{
    public class GetMyConversationsInfoQueryPerformer 
        : IQueryPerformer<GetMyConversationsInfoQuery,IEnumerable<ConversationInfoDTO>>
    {
        private readonly CoreDbContext _db;

        public GetMyConversationsInfoQueryPerformer(CoreDbContext db)
        {
            _db = db;
        }

        public IEnumerable<ConversationInfoDTO> Perform(GetMyConversationsInfoQuery query)
        {
            return _db.ConversationMembers
                .Where(cm => cm.UserId == query.UserId)
                .Join(_db.Conversations, cm => cm.ConversationId, c => c.Id, (cm, c) => c)
                .Select(c => new ConversationInfoDTO
                {
                    ConversationId = c.Id,
                    HasUnreadMessages = true,
                    IsGeoChat = c.IsGeoConversation,
                    Members = _db.ConversationMembers
                        .Where(cm => cm.ConversationId == c.Id)
                        .Join(_db.Users, cm => cm.UserId, u => u.Id, (m, u) =>
                            new ConversationInfoDTO.ConversationMemberInfoDTO
                            {
                                UserId = u.Id,
                                UserName = u.UserName
                            })
                        .ToList()
                })
                .ToList();
        }
    }
}