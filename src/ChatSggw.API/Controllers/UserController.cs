using System;
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
        public IEnumerable<FriendInfoDTO> GetFriendsInfo()
        {
            return _please.Give(new GetMyFriendsInfoQuery
            {
                UserId = Guid.Parse(User.Identity.GetUserId())
            });
        }
    }
}