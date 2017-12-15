using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatSggw.Domain.Structs;

namespace ChatSggw.Domain.DTO
{
    public class MessageModel
    {
        public Guid Id { get; set; }
        public DateTime SendDateTime { get; set; }
        public DateTime DeliveredDateTime { get; set; }
        public DateTime ReadDateTime { get; set; }
        public string Text { get; set; }
        public bool IsLocalized { get; set; }
        public GeoInformation? GeoStamp { get; set; }
    }
}
