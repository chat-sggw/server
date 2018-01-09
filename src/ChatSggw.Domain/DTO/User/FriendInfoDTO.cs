using System;

namespace ChatSggw.Domain.DTO.User
{
    public class FriendInfoDTO
    {
        public Guid Id { get; set; }
        public Guid ConversationId { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public bool HasUnreadMessages { get; set; }
    }
}