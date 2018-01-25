using System;
using ChatSggw.Domain.Structs;

namespace ChatSggw.Domain.DTO.User
{
    public class UserInfoPositionDTO
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }        
        public GeoInformation? GeoStamp { get; set; }
    }
}