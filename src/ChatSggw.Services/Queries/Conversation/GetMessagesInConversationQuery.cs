using System;
using System.Collections.Generic;
using System.Linq;
using ChatSggw.DataLayer;
using ChatSggw.Domain.DTO.Message;
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
            if (!_db.ConversationMembers.Any(cm =>
                cm.UserId == query.UserId && cm.ConversationId == query.ConversationId))
            {
                throw new Exception("Brak uprawnień");
            }

            if (query.AfterMessageId == null && query.BeforeMessageId == null)
            {
                return _db.ConversationMessages
                    .Where(m => m.ConversationId == query.ConversationId)
                    .OrderByDescending(m => m.SendDateTime)
                    .Take(10)
                    .Select(m => new MessageDTO()
                    {
                        Text = m.Text,
                        SendDateTime = m.SendDateTime,
                        AuthorId = m.AuthorId,
                        Id = m.Id,
                        GeoStamp = m.GeoStamp.Longitude != null && m.GeoStamp.Latitude != null
                            ? new GeoInformation()
                            {
                                Longitude = m.GeoStamp.Longitude.Value,
                                Latitude = m.GeoStamp.Latitude.Value,
                            }
                            : (GeoInformation?) null
                    })
                    .AsEnumerable()
                    .Reverse();
            }

            if (query.AfterMessageId != null && query.BeforeMessageId == null)
            {
                var message = _db.ConversationMessages.Find(query.ConversationId, query.AfterMessageId);
                if (message == null)
                {
                    throw new Exception("Nieprawidłowe id wiadomości");
                }
                return _db.ConversationMessages
                    .Where(m => m.ConversationId == query.ConversationId)
                    .Where(m => m.SendDateTime > message.SendDateTime)
                    .OrderBy(m => m.SendDateTime)
                    .Take(10)
                    .Select(m => new MessageDTO()
                    {
                        Text = m.Text,
                        SendDateTime = m.SendDateTime,
                        AuthorId = m.AuthorId,
                        Id = m.Id,
                        GeoStamp = m.GeoStamp.Longitude != null && m.GeoStamp.Latitude != null
                            ? new GeoInformation()
                            {
                                Longitude = m.GeoStamp.Longitude.Value,
                                Latitude = m.GeoStamp.Latitude.Value,
                            }
                            : (GeoInformation?) null
                    })
                    .AsEnumerable();
            }

            if (query.AfterMessageId == null && query.BeforeMessageId != null)
            {
                var message = _db.ConversationMessages.Find(query.ConversationId, query.BeforeMessageId);
                if (message == null)
                {
                    throw new Exception("Nieprawidłowe id wiadomości");
                }
                return _db.ConversationMessages
                    .Where(m => m.ConversationId == query.ConversationId)
                    .Where(m => m.SendDateTime < message.SendDateTime)
                    .OrderByDescending(m => m.SendDateTime)
                    .Take(10)
                    .Select(m => new MessageDTO()
                    {
                        Text = m.Text,
                        SendDateTime = m.SendDateTime,
                        AuthorId = m.AuthorId,
                        Id = m.Id,
                        GeoStamp = m.GeoStamp.Longitude != null && m.GeoStamp.Latitude != null
                            ? new GeoInformation()
                            {
                                Longitude = m.GeoStamp.Longitude.Value,
                                Latitude = m.GeoStamp.Latitude.Value,
                            }
                            : (GeoInformation?) null
                    })
                    .AsEnumerable()
                    .Reverse();
            }

            throw new InvalidOperationException(
                $"Nie można wykonać query, podano jednocześnie pola {nameof(GetMessagesInConversationQuery.AfterMessageId)} " +
                $"i {nameof(GetMessagesInConversationQuery.BeforeMessageId)}");
        }
    }
}