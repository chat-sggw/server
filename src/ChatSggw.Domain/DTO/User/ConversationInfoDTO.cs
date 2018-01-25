using System;
using System.Collections.Generic;

namespace ChatSggw.Domain.DTO.User
{
    public class ConversationInfoDTO
    {
        public List<ConversationMemberInfoDTO> Members { get; set; }
        public Guid ConversationId { get; set; }
        public bool IsGeoChat { get; set; }
        public bool HasUnreadMessages { get; set; }

        public class ConversationMemberInfoDTO
        {
            public Guid UserId { get; set; }
            public string UserName { get; set; }
        }
    }
}