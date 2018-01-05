using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ChatSggw.Domain.Commands.Conversation;
using ChatSggw.Domain.Commands.Message;
using ChatSggw.Domain.DTO.Message;
using ChatSggw.Domain.Entities.Conversation;
using ChatSggw.Domain.Queries.Conversation;
using ChatSggw.Domain.Structs;
using Microsoft.AspNet.Identity;
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

        /// <summary>
        /// Metoda tworzy nową konwersację grupową
        /// </summary>
        /// <param name="members">Lista identyfikatorów przyjaciół których chcemy dodać do nowo utworzonej grupy</param>
        /// <returns>Id konwersacji grupowej</returns>
        [HttpPost]
        [Route("create")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(IEnumerable<ValidationError>))]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(Guid))]
        public HttpResponseMessage Create([FromBody] Guid[] members)
        {
            var create = new CreateGroupConversationCommand()
            {
                Members = members,
                UserId = Guid.Parse(User.Identity.GetUserId())
            };

            //            var commandResult = _please.Do(create);
            //
            //            return commandResult.WasSuccessful()
            //                ? Request.CreateResponse(HttpStatusCode.OK, "ok")
            //                : Request.CreateResponse(HttpStatusCode.BadRequest, commandResult.ValidationErrors);
            return Request.CreateResponse(HttpStatusCode.OK, Guid.Empty);
        }

        /// <summary>
        /// Metoda dodaje przyjaciela do konwersacji grupowej
        /// </summary>
        /// <param name="memberId">Identyfikator użytkownika który jest przyjacielem użytkownika</param>
        /// <param name="conversationId">Identyfikator konwersacji</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{conversationId:guid}/add-member")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(IEnumerable<ValidationError>))]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(string))]
        public HttpResponseMessage AddMember([FromBody] Guid memberId, Guid conversationId)
        {
            var addMember = new AddMemberToConversationCommand
            {
                MemberId = memberId,
                ConversationId = conversationId,
                UserId = Guid.Parse(User.Identity.GetUserId())
            };

            var commandResult = _please.Do(addMember);

            return commandResult.WasSuccessful()
                ? Request.CreateResponse(HttpStatusCode.OK, "ok")
                : Request.CreateResponse(HttpStatusCode.BadRequest, commandResult.ValidationErrors);
        }

        /// <summary>
        /// Metoda usuwa członka konwersacjo
        /// </summary>
        /// <param name="memberId">Identyfikator członka konwersacji</param>
        /// <param name="conversationId">Identyfikator konwersacji</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{conversationId:guid}/remove-member")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(IEnumerable<ValidationError>))]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(string))]
        public HttpResponseMessage RemoveMember([FromBody] Guid memberId, Guid conversationId)
        {
            var addMember = new RemoveMemberFromConversationCommand()
            {
                MemberId = memberId,
                ConversationId = conversationId,
                UserId = Guid.Parse(User.Identity.GetUserId())
            };

//            var commandResult = _please.Do(addMember);
//
//            return commandResult.WasSuccessful()
//                ? Request.CreateResponse(HttpStatusCode.OK, "ok")
//                : Request.CreateResponse(HttpStatusCode.BadRequest, commandResult.ValidationErrors);

            return Request.CreateResponse(HttpStatusCode.OK, "ok");
        }

        [HttpGet]
        [Route("{conversationId:guid}/search")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(IEnumerable<ValidationError>))]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(string))]
        public IEnumerable<MessageDTO> Search(Guid conversationId, [FromUri] string query)
        {
            var retrieveMessages = new SearchForMessagesInConversationQuery()
            {
                ConversationId = conversationId,
                QueryString = query,
                UserId = Guid.Parse(User.Identity.GetUserId())
            };
//            var messageDtos = _please.Give(retrieveMessages);
            IEnumerable<MessageDTO> messageDtos = new List<MessageDTO>
            {
                new MessageDTO
                {
                    Id = Guid.Empty,
                    Text = "Hejka",
                    AuthorId = Guid.Empty,
                    GeoStamp = new GeoInformation
                    {
                        Longitude = 10,
                        Latitude = 10
                    },
                    SendDateTime = DateTime.Now.AddMinutes(-1)
                },
                new MessageDTO
                {
                    Id = Guid.Empty,
                    Text = "No siema",
                    AuthorId = Guid.Empty,
                    GeoStamp = new GeoInformation
                    {
                        Longitude = 10,
                        Latitude = 10
                    },
                    SendDateTime = DateTime.Now
                }
            };
            return messageDtos;
        }

        /// <summary>
        /// Metoda pobiera wiadomości z czatu o podanym id 
        /// </summary>
        /// <param name="conversationId">Id czatu</param>
        /// <param name="afterMessage">Id najświeższej posiadanej wiadomości. Parametr opcjonalny, nie można go łączyć z <see cref="beforeMessage"/> </param>
        /// <param name="beforeMessage">Id najstarszej posiadanej wiadomości. Parametr opcjonalny, nie można go łączyć z <see cref="afterMessage"/> </param>
        /// <returns></returns>
        [HttpGet]
        [Route("{conversationId:guid}")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(IEnumerable<ValidationError>))]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(string))]
        public IEnumerable<MessageDTO> GetMessages(Guid conversationId, [FromUri] Guid? afterMessage,
            [FromUri] Guid? beforeMessage)
        {
            var retrieveMessages = new GetMessagesInConversationQuery()

            {
                ConversationId = conversationId,
                AfterMessageId = afterMessage,
                BeforeMessageId = beforeMessage,
                UserId = Guid.Parse(User.Identity.GetUserId())
            };
//            var messageDtos = _please.Give(retrieveMessages);
            IEnumerable<MessageDTO> messageDtos = new List<MessageDTO>
            {
                new MessageDTO
                {
                    Id = Guid.Empty,
                    Text = "Hejka",
                    AuthorId = Guid.Empty,
                    GeoStamp = new GeoInformation
                    {
                        Longitude = 10,
                        Latitude = 10
                    },
                    SendDateTime = DateTime.Now.AddMinutes(-1)
                },
                new MessageDTO
                {
                    Id = Guid.Empty,
                    Text = "No siema",
                    AuthorId = Guid.Empty,
                    GeoStamp = new GeoInformation
                    {
                        Longitude = 10,
                        Latitude = 10
                    },
                    SendDateTime = DateTime.Now
                }
            };
            return messageDtos;
        }

        [HttpPost]
        [Route("{conversationId:guid}/send")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(IEnumerable<ValidationError>))]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(Guid), Description = "Returns messeage ID.")]
        public HttpResponseMessage Send([FromBody] string text, Guid conversationId)
        {
            var messageId = new Guid();
            var sendMessage = new SendMessageCommand
            {
                ConversationId = conversationId,
                MemberId = Guid.Parse(User.Identity.GetUserId()),
                MessageId = messageId,
                Text = text
            };

            //var commandResult = _please.Do(sendMessage);

            //            return commandResult.WasSuccessful()
            //                ? Request.CreateResponse(HttpStatusCode.OK, messageId);
            //                : Request.CreateResponse(HttpStatusCode.BadRequest, commandResult.ValidationErrors);
            return Request.CreateResponse(HttpStatusCode.OK, messageId);
        }
    }
}