using System;
using System.Collections.Generic;
using ChatSggw.Domain.DTO.User;
using Neat.CQRSLite.Contract.Queries;

namespace ChatSggw.Domain.Queries.Conversation
{
    public class GetMyConversationsInfoQuery : IQuery<IEnumerable<ConversationInfoDTO>>
    {
        public Guid UserId { get; set; }
    }
}