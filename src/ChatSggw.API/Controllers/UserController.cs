﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ChatSggw.Domain.Commands.User;
using ChatSggw.Domain.DTO.User;
using ChatSggw.Domain.Queries.User;
using ChatSggw.Domain.Structs;
using Microsoft.AspNet.Identity;
using Neat.CQRSLite.Contract.Commands;
using Neat.CQRSLite.Contract.Helpers;
using Swashbuckle.Swagger.Annotations;

namespace ChatSggw.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        private readonly Assistant _please;

        public UserController(Assistant please)
        {
            _please = please;
        }

        [HttpPost]
        [Route("ping")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(IEnumerable<ValidationError>))]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(string))]
        public HttpResponseMessage Ping([FromBody] double longitude, [FromBody] double latitude)
        {
            var pingUserLocation = new PingUserLocationCommand
            {
                Location = new GeoInformation
                {
                    Longitude = longitude,
                    Latitude = latitude
                },
                UserId = Guid.Parse(User.Identity.GetUserId())
            };

            var commandResult = _please.Do(pingUserLocation);

            return commandResult.WasSuccessful()
                ? Request.CreateResponse(HttpStatusCode.OK, "ok")
                : Request.CreateResponse(HttpStatusCode.BadRequest, commandResult.ValidationErrors);
        }

        [HttpPost]
        [Route("friends/{id:guid}")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(IEnumerable<ValidationError>))]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(string))]
        public HttpResponseMessage AddFriend(Guid id)
        {
            var command = new AddFriendCommand
            {
                SecondUserId = id,
                FirstUserId = Guid.Parse(User.Identity.GetUserId())
            };

            var commandResult = _please.Do(command);

            return commandResult.WasSuccessful()
                ? Request.CreateResponse(HttpStatusCode.OK, "ok")
                : Request.CreateResponse(HttpStatusCode.BadRequest, commandResult.ValidationErrors);
        }

        [HttpDelete]
        [Route("friends/{id:guid}")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(IEnumerable<ValidationError>))]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(string))]
        public HttpResponseMessage RemoveFriend(Guid id)
        {
//            var command = new AddFriendCommand()
//            {
//                SecondUserId = id,
//                UserId = Guid.Parse(User.Identity.GetUserId())
//            };

            return Request.CreateResponse(HttpStatusCode.OK, "ok");
        }

        [HttpGet]
        [Route("search")]
        public IEnumerable<UserInfoDTO> Search([FromUri] string query)
        {
            var q = new SearchUserQuery
            {
                QueryString = query,
                UserId = Guid.Parse(User.Identity.GetUserId())
            };

            IEnumerable<UserInfoDTO> users = new List<UserInfoDTO>
            {
                new UserInfoDTO
                {
                    Id = Guid.Empty,
                    IsActive = true,
                    UserName = "Mietek"
                }
            };

            return users;
        }

        //todo: do zrobienia tak, zeby pracowalo z FriendsPairDTO
        [HttpGet]
        [Route("friends")]
        public IEnumerable<FriendInfoDTO> GetFriendsInfo()
        {
            var q = new GetMyFriendsInfoQuery
            {
                UserId = Guid.Parse(User.Identity.GetUserId())
            };

            IEnumerable<FriendInfoDTO> friends = new List<FriendInfoDTO>
            {
                new FriendInfoDTO
                {
                    Id = Guid.Empty,
                    IsActive = true,
                    UserName = "Mietek",
                    ConversationId = Guid.Empty,
                    HasUnreadMessages = true
                }
            };

            return friends;
        }
    }
}