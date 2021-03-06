﻿using ChatSggw.Domain.Commands.FriendsPair;
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

            var commandResult = _please.Do(new AddFriendCommand
            {
                UserId = Guid.Parse(User.Identity.GetUserId()),
                FriendId = id
            });

            return commandResult.WasSuccessful()
                ? Request.CreateResponse(HttpStatusCode.OK, "Użytkownik został pomyślnie dodany do listy znajomych")
                : Request.CreateResponse(HttpStatusCode.BadRequest, commandResult.ValidationErrors);
        }

        [HttpDelete]
        [Route("friends/{id:guid}")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(IEnumerable<ValidationError>))]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(string))]
        public HttpResponseMessage RemoveFriend(Guid id)
        {
            var commandResult = _please.Do(new RemoveFriendCommand
            {
                UserId = Guid.Parse(User.Identity.GetUserId()),
                FriendId = id
            });

            return commandResult.WasSuccessful()
                ? Request.CreateResponse(HttpStatusCode.OK, "Użytkownik został usunięty z grona znajomych")
                : Request.CreateResponse(HttpStatusCode.BadRequest, commandResult.ValidationErrors);
        }
    }
}