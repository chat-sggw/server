using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ChatSggw.Domain.Commands.Conversation;
using ChatSggw.Domain.Commands.Message;
using ChatSggw.Domain.Entities.Conversation;
using ChatSggw.Domain.Structs;
using Neat.CQRSLite.Contract.Commands;
using Neat.CQRSLite.Contract.Helpers;
using Swashbuckle.Swagger.Annotations;

namespace ChatSggw.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/conversation")]
    public class ConversationController : ApiController
    {
        private readonly Assistant _please;

        public ConversationController(Assistant please)
        {
            _please = please;
        }

        [HttpPost]
        [Route("create")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(IEnumerable<ValidationError>))]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(string))]
        public HttpResponseMessage Create([FromBody] params Guid[] members)
        {
            var create = new CreateGroupConversationCommand()
            {
                Conversation = new Conversation()
            };

            //todo [KR]: dodac inicjalizacje memberow z listy guidow z body

            var commandResult = _please.Do(create);

            return commandResult.WasSuccessful()
                ? Request.CreateResponse(HttpStatusCode.OK, "ok")
                : Request.CreateResponse(HttpStatusCode.BadRequest, commandResult.ValidationErrors);
        }

        [HttpPost]
        [Route("addMember")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(IEnumerable<ValidationError>))]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(string))]
        public HttpResponseMessage AddMember([FromBody] Guid userId, [FromBody] Guid conversationId)
        {
            var addMember = new AddMemberToConversationComand
            {
                MemberId = userId,
                ConversationId = conversationId
            };

            var commandResult = _please.Do(addMember);

            return commandResult.WasSuccessful()
                ? Request.CreateResponse(HttpStatusCode.OK, "ok")
                : Request.CreateResponse(HttpStatusCode.BadRequest, commandResult.ValidationErrors);
        }

        [HttpPost]
        [Route("removeMember")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(IEnumerable<ValidationError>))]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(string))]
        public HttpResponseMessage RemoveMember([FromBody] Guid userId, [FromBody] Guid conversationId)
        {
            var addMember = new RemoveMemberFromConversationCommand()
            {
                MemberId = userId,
                ConversationId = conversationId
            };

            var commandResult = _please.Do(addMember);

            return commandResult.WasSuccessful()
                ? Request.CreateResponse(HttpStatusCode.OK, "ok")
                : Request.CreateResponse(HttpStatusCode.BadRequest, commandResult.ValidationErrors);
        }
    }
}