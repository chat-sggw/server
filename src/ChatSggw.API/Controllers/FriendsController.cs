using ChatSggw.API.Helpers;
using ChatSggw.Domain.Commands.FriendsPair;
using Microsoft.AspNet.Identity;
using Neat.CQRSLite.Contract.Commands;
using Neat.CQRSLite.Contract.Helpers;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ChatSggw.API.Controllers
{
    public class FriendsController : ApiController
    {
        private readonly Assistant _please;

        public FriendsController(Assistant please)
        {
            _please = please;
        }

        [HttpPost]
        [Route("friends/{id:guid}")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(IEnumerable<ValidationError>))]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(string))]
        public HttpResponseMessage AddFriend(Guid id)
        {
            var command = new AddFriendCommand
            {
                UserId = Guid.Parse(User.Identity.GetUserId()),
                FriendId = id
            };

            return command.ProceesForResult(_please, Request);
        }

        [HttpDelete]
        [Route("friends/{id:guid}")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(IEnumerable<ValidationError>))]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(string))]
        public HttpResponseMessage RemoveFriend(Guid id)
        {
            var command = new RemoveFriendCommand
            {
                UserId = Guid.Parse(User.Identity.GetUserId()),
                FriendId = id
            };

            return command.ProceesForResult(_please, Request);
        }
    }
}