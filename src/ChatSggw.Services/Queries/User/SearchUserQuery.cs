using System;
using System.Collections.Generic;
using System.Linq;
using ChatSggw.DataLayer;
using ChatSggw.Domain.DTO.User;
using ChatSggw.Domain.Queries.User;
using Neat.CQRSLite.Contract.Queries;

namespace ChatSggw.Services.Queries.User
{
    public class SearchUserQueryPerformer : IQueryPerformer<SearchUserQuery, IEnumerable<UserInfoDTO>>
    {
        private readonly CoreDbContext _db;

        public SearchUserQueryPerformer(CoreDbContext db)
        {
            _db = db;
        }

        public IEnumerable<UserInfoDTO> Perform(SearchUserQuery query)
        {
            var users = _db.Users
                .Where(user => user.UserName.Contains(query.QueryString) || user.Email.Contains(query.QueryString))
                .Select(user => new UserInfoDTO
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    IsActive = true, //todo
                })
                .ToList();

            var ids = users.Select(u => u.Id).ToList();


            var userIdsToExclude = _db.FriendsPairs.Where(pair =>
                    (pair.FirstUserId == query.UserId && ids.Contains(pair.SecondUserId)) ||
                    (pair.SecondUserId == query.UserId && ids.Contains(pair.FirstUserId)))
                .Select(p => p.FirstUserId != query.UserId ? p.FirstUserId : p.SecondUserId);

            return users
                .Where(u => !userIdsToExclude.Contains(u.Id))
                .ToList();
        }
    }
}