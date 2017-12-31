using ChatSggw.Domain.Commands.Message;
using ChatSggw.Domain.Structs;
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
    [Authorize]
    [RoutePrefix("api/message")]
    public class MessageController : ApiController
    {
        private readonly Assistant _please;

        public MessageController(Assistant please)
        {
            _please = please;
        }

        [HttpPost]
        [Route("send")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(IEnumerable<ValidationError>))]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(string))]
        public HttpResponseMessage Send([FromBody] string text, 
            [FromBody] Guid userId, [FromBody] Guid conversationId)
        {
            var sendMessage = new SendMessageCommand
            {
                //todo [KR]: wymaga jakiegos sensownego uzupelnienia
                GeoStamp = new GeoInformation(),
                ConversationId = conversationId,
                MemberId = userId,
                MessageId = new Guid(),
                Text = text
            };

            var commandResult = _please.Do(sendMessage);

            return commandResult.WasSuccessful()
                ? Request.CreateResponse(HttpStatusCode.OK, "ok")
                : Request.CreateResponse(HttpStatusCode.BadRequest, commandResult.ValidationErrors);
        }
    }
}