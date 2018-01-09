using System;
using System.Collections.Generic;
using System.Linq;
using ChatSggw.DataLayer;
using ChatSggw.Domain.DTO.Message;
using ChatSggw.Domain.Queries.Conversation;
using ChatSggw.Domain.Structs;
using Neat.CQRSLite.Contract.Queries;

namespace ChatSggw.Services.Queries.Conversation
{
    public class
        SearchForMessagesInConversationQueryPerformer : IQueryPerformer<SearchForMessagesInConversationQuery,
            IEnumerable<MessageDTO>>
    {
        public string QueryString { get; set; }
        public Guid ConversationId { get; set; }
        public Guid UserId { get; set; }

        private readonly CoreDbContext _db;

        public SearchForMessagesInConversationQueryPerformer(CoreDbContext db)
        {
            _db = db;
        }

        public IEnumerable<MessageDTO> Perform(SearchForMessagesInConversationQuery query)
        {
            if (!_db.ConversationMembers.Any(cm =>
                cm.UserId == query.UserId && cm.ConversationId == query.ConversationId))
            {
                throw new Exception("Brak uprawnień");
            }

            var messages = _db.ConversationMessages
                .Where(m => m.ConversationId == query.ConversationId)
                .OrderByDescending(m => m.SendDateTime)
                .Select(m => new MessageDTO()
                {
                    Text = m.Text,
                    SendDateTime = m.SendDateTime,
                    AuthorId = m.AuthorId,
                    Id = m.Id,
                    GeoStamp = m.GeoStamp,
                })
                .Take(101) // limit to 20 max messages
                .ToList();

            return messages.Count == 101
                ? new List<MessageDTO>()
                : messages;
        }
    }
}