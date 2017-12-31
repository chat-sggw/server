using System;
using System.Data.Entity.Spatial;
using ChatSggw.Domain.Structs;

namespace ChatSggw.Domain.DTO.Message
{
    public class MessageDTO
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public Guid AuthorId { get; private set; }
        public DateTime SendDateTime { get; private set; }
        public GeoInformation GeoStamp { get; private set; }
    }
}
