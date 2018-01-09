using ChatSggw.Domain.Structs;
using System;

namespace ChatSggw.Domain.DTO.Message
{
    public class MessageDTO
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public Guid AuthorId { get;  set; }
        public DateTime SendDateTime { get;  set; }
        public GeoInformation? GeoStamp { get;  set; }
    }
}
