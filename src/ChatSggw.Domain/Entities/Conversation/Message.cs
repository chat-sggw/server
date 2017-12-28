using System;
using System.Data.Entity.Spatial;
using ChatSggw.Domain.Structs;
using Neat.CQRSLite.Contract.Domain;

namespace ChatSggw.Domain.Entities.Conversation
{
    public class Message
    {
        //key
        public Guid ConversationId { get; private set; }
        public Guid Id { get; private set; }

        //rest
        public Guid AuthorId { get; private set; }
        public DateTime SendDateTime { get; private set; }
        public string Text { get; private set; }
        public bool IsLocalized => GeoStamp != null;
        public DbGeography GeoStamp { get; private set; }


        public static Message Create(string text, Guid conversationId, Guid authorId, GeoInformation? geoStamp = null)
        {
            var message = new Message
            {
                Id = Guid.NewGuid(),
                Text = text,
                AuthorId = authorId,
                ConversationId = conversationId,
                SendDateTime = DateTime.UtcNow
            };
            if (geoStamp.HasValue)
            {
                message.GeoStamp = DbGeographyHelper
                    .ConvertLatLonToDbGeography(geoStamp.Value.Longitude, geoStamp.Value.Latitude);
            }
            return message;
        }
    }
}