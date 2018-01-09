using System;
using System.Collections.Generic;
using ChatSggw.Domain.DTO.User;
using Neat.CQRSLite.Contract.Queries;

namespace ChatSggw.Domain.Queries.User
{
    public class SearchUserQuery : IQuery<IEnumerable<UserInfoDTO>>
    {
        public string QueryString { get; set; }
        public Guid UserId { get; set; }
    }
}