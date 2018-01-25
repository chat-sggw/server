using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.Linq;
using ChatSggw.DataLayer;
using ChatSggw.DataLayer.IdentityModels;
using ChatSggw.Domain.DTO.Message;
using ChatSggw.Domain.Entities.Conversation;
using ChatSggw.Domain.Queries.Conversation;
using ChatSggw.Domain.Structs;
using Neat.CQRSLite.Contract.Queries;

namespace ChatSggw.Services.Queries.Conversation
{
    public class
        GetMessagesInConversationQueryPerformer : IQueryPerformer<GetMessagesInConversationQuery,
            IEnumerable<MessageDTO>>
    {
        private readonly CoreDbContext _db;

        public GetMessagesInConversationQueryPerformer(CoreDbContext db)
        {
            _db = db;
        }

        public IEnumerable<MessageDTO> Perform(GetMessagesInConversationQuery query)
        {
            var user = _db.Users.Find(query.UserId);
            var conversationMember = _db.ConversationMembers.SingleOrDefault(cm =>
                cm.UserId == query.UserId && cm.ConversationId == query.ConversationId)
                ?? throw new Exception("Brak uprawnień");
            var conversation = _db.Conversations.Find(query.ConversationId)
                ?? throw new Exception("Nie istnieje taka konwersacja");


            IQueryable<Message> messagesQuery = _db.ConversationMessages
                .Where(m => m.ConversationId == query.ConversationId);

            List<Message> messages;

            if (conversation.IsGeoConversation)
            {
                if (user.CurrentPosition == null)
                {
                    throw new Exception("Brak ustawionej geolokalizacji");
                }
                FilterByDistance(ref messagesQuery, conversationMember.GeoConversationRange , user.CurrentPosition.Value);
            }


            if (query.AfterMessageId == null && query.BeforeMessageId == null)
                messages = messagesQuery
                    .OrderByDescending(m => m.SendDateTime)
                    .Take(10)
                    .AsEnumerable() // download from db
                    .Reverse()
                    .ToList();

            else if (query.AfterMessageId != null && query.BeforeMessageId == null)
            {
                var message = _db.ConversationMessages.Find(query.ConversationId, query.AfterMessageId);
                if (message == null)
                {
                    throw new Exception("Nieprawidłowe id wiadomości");
                }
                messages = messagesQuery
                    .Where(m => m.SendDateTime > message.SendDateTime)
                    .OrderBy(m => m.SendDateTime)
                    .Take(10)
                    .ToList();
            }

            else if (query.AfterMessageId == null && query.BeforeMessageId != null)
            {
                var message = _db.ConversationMessages.Find(query.ConversationId, query.BeforeMessageId);
                if (message == null)
                {
                    throw new Exception("Nieprawidłowe id wiadomości");
                }
                messages = messagesQuery
                    .Where(m => m.SendDateTime < message.SendDateTime)
                    .OrderByDescending(m => m.SendDateTime)
                    .Take(10)
                    .AsEnumerable() // download from db
                    .Reverse()
                    .ToList();
            }
            else
            {
                throw new InvalidOperationException(
                    $"Nie można wykonać query, podano jednocześnie pola {nameof(GetMessagesInConversationQuery.AfterMessageId)} " +
                    $"i {nameof(GetMessagesInConversationQuery.BeforeMessageId)}");
            }

            return messages
                .Select(m => new MessageDTO
                {
                    Text = m.Text,
                    SendDateTime = m.SendDateTime,
                    AuthorId = m.AuthorId,
                    Id = m.Id,
                    GeoStamp = m.GeoStamp,
                    Distance = m.GeoStamp.HasValue && user.CurrentPosition.HasValue
                        ? m.GeoStamp.Value.GetDistance(user.CurrentPosition.Value)
                        : (double?) null,
                    
                })
                .ToList();
        }

        protected void FilterByDistance(ref IQueryable<Message> messagesQuery, double maxDistance, GeoInformation userLocation)
        {
            double lon = userLocation.Longitude;
            double lat = userLocation.Latitude;

            const double p = 0.017453292519943295; // Math.PI / 180


            messagesQuery = messagesQuery
                .Where(m => 12742 * SqlFunctions.Asin(SqlFunctions.SquareRoot(
                                0.5 - SqlFunctions.Cos((m.GeoStampLatitude.Value - lat) * p) / 2 +
                                SqlFunctions.Cos(lat * p) * SqlFunctions.Cos(m.GeoStampLatitude.Value * p) *
                                (1 - SqlFunctions.Cos((m.GeoStampLongitude.Value - lon) * p)) / 2)) <
                            maxDistance);
        }
    }
}