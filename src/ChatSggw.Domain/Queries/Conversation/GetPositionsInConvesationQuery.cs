using System;
using System.Collections.Generic;
using ChatSggw.Domain.DTO.User;
using Neat.CQRSLite.Contract.Queries;

namespace ChatSggw.Domain.Queries.Conversation
{
    public class GetPositionsInConvesationQuery : IQuery<IEnumerable<UserInfoPositionDTO>>
    {               
        public Guid ConversationId { get; set; }
        public Guid UserId { get; set; }
    }
}