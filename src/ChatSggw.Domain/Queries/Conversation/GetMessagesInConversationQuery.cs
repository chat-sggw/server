using System;
using System.Collections.Generic;
using ChatSggw.Domain.DTO.Message;
using Neat.CQRSLite.Contract.Queries;

namespace ChatSggw.Domain.Queries.Conversation
{
    public class GetMessagesInConversationQuery : IQuery<IEnumerable<MessageDTO>>
    {
        /// <summary>
        /// Jeżeli podane, zwraca wiadomości napisane później niż wiadomość o podanym ID. 
        /// Używane do pobrania najnowszych wiadomości.
        /// Wyklucza się z <see cref="BeforeMessageId"/> 
        /// </summary>
        public Guid? AfterMessageId{ get; set; }
        /// <summary>
        /// Jeżeli podane, zwraca wiadomości napisane wcześniej niż wiadomość o podanym ID. 
        /// Używane do pobrania historii konwersacji.
        /// Wyklucza się z <see cref="AfterMessageId"/> 
        /// </summary>
        public Guid? BeforeMessageId{ get; set; }
        public Guid ConversationId { get; set; }
        public Guid UserId { get; set; }
    }
}