using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.AccessControl;
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
        /// <returns>Id konwersacji grupowej</returns>
        [HttpPost]
        [Route("create")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(IEnumerable<ValidationError>))]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(Guid))]
        public HttpResponseMessage Create(CreateGroupModel model)
        {
            var commandResult = _please.Do(new CreateGroupConversationCommand
            {
                Members = model.Members,
                IsGeoConversation = model.IsGeoChat,
                UserId = Guid.Parse(User.Identity.GetUserId())
            });

            return commandResult.WasSuccessful()
                ? Request.CreateResponse(HttpStatusCode.OK, "ok")
                : Request.CreateResponse(HttpStatusCode.BadRequest, commandResult.ValidationErrors);
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
        public HttpResponseMessage AddMember(Guid memberId, Guid conversationId)
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
        /// Metoda usuwa członka konwersacji
        /// </summary>
        /// <param name="memberId">Identyfikator członka konwersacji</param>
        /// <param name="conversationId">Identyfikator konwersacji</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{conversationId:guid}/remove-member")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(IEnumerable<ValidationError>))]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(string))]
        public HttpResponseMessage RemoveMember(Guid memberId, Guid conversationId)
        {

            var commandResult = _please.Do(new RemoveMemberFromConversationCommand()
            {
                MemberId = memberId,
                ConversationId = conversationId,
                UserId = Guid.Parse(User.Identity.GetUserId())
            });

            return commandResult.WasSuccessful()
                ? Request.CreateResponse(HttpStatusCode.OK, "ok")
                : Request.CreateResponse(HttpStatusCode.BadRequest, commandResult.ValidationErrors);
        }

        [HttpGet]
        [Route("{conversationId:guid}/search")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(IEnumerable<ValidationError>))]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(string))]
        public IEnumerable<MessageDTO> Search(Guid conversationId, [FromUri] string query)
        {
            return _please.Give(new SearchForMessagesInConversationQuery()
            {
                ConversationId = conversationId,
                QueryString = query,
                UserId = Guid.Parse(User.Identity.GetUserId())
            });
        }

        /// <summary>
        /// Metoda pobiera wiadomości z czatu o podanym id 
        /// </summary>
        /// <param name="conversationId">Id czatu</param>
        /// <param name="afterMessage">Id najświeższej posiadanej wiadomości. Parametr opcjonalny.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{conversationId:guid}")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(IEnumerable<ValidationError>))]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(string))]
        public IEnumerable<MessageDTO> GetMessages(Guid conversationId, [FromUri] Guid? afterMessage = null)
        {
            return _please.Give(new GetMessagesInConversationQuery()

            {
                ConversationId = conversationId,
                AfterMessageId = afterMessage,
                UserId = Guid.Parse(User.Identity.GetUserId())
            });
        }

        /// <summary>
        /// Metoda pobiera starsze wiadomości z czatu o podanym id 
        /// </summary>
        /// <param name="conversationId">Id czatu</param>
        /// <param name="beforeMessage">Id najstarszej wiadomości</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{conversationId:guid}/history")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(IEnumerable<ValidationError>))]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(string))]
        public IEnumerable<MessageDTO> GetMessages(Guid conversationId, [FromUri] Guid beforeMessage)
        {
            return _please.Give(new GetMessagesInConversationQuery()

            {
                ConversationId = conversationId,
                BeforeMessageId = beforeMessage,
                UserId = Guid.Parse(User.Identity.GetUserId())
            });
        }

        [HttpPost]
        [Route("{conversationId:guid}/send")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(IEnumerable<ValidationError>))]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(Guid), Description = "Returns messeage ID.")]
        public HttpResponseMessage Send([FromBody] SendMessageModel model, Guid conversationId)
        {
            var messageId = Guid.NewGuid();

            var commandResult = _please.Do(new SendMessageCommand
            {
                ConversationId = conversationId,
                UserId = Guid.Parse(User.Identity.GetUserId()),
                MessageId = messageId,
                Text = model.Text,
            });

            return commandResult.WasSuccessful()
                ? Request.CreateResponse(HttpStatusCode.OK, messageId)
                : Request.CreateResponse(HttpStatusCode.BadRequest, commandResult.ValidationErrors);
        }

        public class SendMessageModel
        {
            public string Text { get; set; }
        }

        public class CreateGroupModel
        {
            /// <summary>
            /// Lista identyfikatorów przyjaciół których chcemy dodać do nowo utworzonej grupy
            /// </summary>
            public Guid[] Members { get; set; }

            /// <summary>
            /// Określa czy czat ma być konwersacją grupową
            /// </summary>
            public bool IsGeoChat { get; set; }
        }
    }
}