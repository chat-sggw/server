using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ChatSggw.Domain.Commands.User;
using ChatSggw.Domain.DTO.User;
using ChatSggw.Domain.Queries.Conversation;
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
        public HttpResponseMessage Ping([FromBody] GeoInformation geoInformation)
        {
            var pingUserLocation = new PingUserLocationCommand
            {
                Location = geoInformation,
                UserId = Guid.Parse(User.Identity.GetUserId())
            };

            var commandResult = _please.Do(pingUserLocation);

            return commandResult.WasSuccessful()
                ? Request.CreateResponse(HttpStatusCode.OK, "ok")
                : Request.CreateResponse(HttpStatusCode.BadRequest, commandResult.ValidationErrors);
        }

        [HttpGet]
        [Route("search")]
        public IEnumerable<UserInfoDTO> Search([FromUri] string query)
        {
            return _please.Give(new SearchUserQuery
            {
                QueryString = query,
                UserId = Guid.Parse(User.Identity.GetUserId())
            });
        }

        [HttpGet]
        [Route("friends")]
        public ChatInfo GetFriendsInfo()
        {
            return new ChatInfo
            {
                Friends = _please.Give(new GetMyFriendsInfoQuery {UserId = Guid.Parse(User.Identity.GetUserId())}),
                Conversations =
                    _please.Give(new GetMyConversationsInfoQuery {UserId = Guid.Parse(User.Identity.GetUserId())})
            };
        }

        public class ChatInfo
        {
            public IEnumerable<FriendInfoDTO> Friends { get; set; }
            public IEnumerable<ConversationInfoDTO> Conversations { get; set; }
        }
    }
}