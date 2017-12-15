using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatSggw.Domain.Structs;

namespace ChatSggw.Domain.Entities
{
    public class Message
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime SendDateTime { get; set; }
        public DateTime DeliveredDateTime { get; set; }
        public DateTime ReadDateTime { get; set; }
        public string Text { get; set; }
        public bool IsLocalized { get; set; }
        public GeoInformation? GeoStamp { get; set; }
    }
}
