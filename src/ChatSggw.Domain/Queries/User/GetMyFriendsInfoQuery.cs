using System;
using System.Collections.Generic;
using ChatSggw.Domain.DTO.User;
using Neat.CQRSLite.Contract.Queries;

namespace ChatSggw.Domain.Queries.User
{
    public class GetMyFriendsInfoQuery : IQuery<IEnumerable<FriendInfoDTO>>
    {
        public Guid UserId { get; set; }
    }
}