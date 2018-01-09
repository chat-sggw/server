using System;

namespace ChatSggw.Domain.DTO.User
{
    public class UserInfoDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
    }
}