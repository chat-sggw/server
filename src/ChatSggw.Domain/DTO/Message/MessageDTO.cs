using ChatSggw.Domain.Structs;
using System;

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
