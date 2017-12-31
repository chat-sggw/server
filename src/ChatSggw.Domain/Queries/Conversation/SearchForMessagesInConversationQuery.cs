using Neat.CQRSLite.Contract.Queries;
using System;
using System.Collections.Generic;
using ChatSggw.Domain.DTO.Message;

namespace ChatSggw.Domain.Queries.Conversation
{
    public class SearchForMessagesInConversationQuery : IQuery<IEnumerable<MessageDTO>>
    {
        public string QueryString { get; set; }
        public Guid ConversationId { get; set; }
    }
}
