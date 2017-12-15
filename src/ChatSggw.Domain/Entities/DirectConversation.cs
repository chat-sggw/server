using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatSggw.Domain.Entities
{
    //We may consider some basic property to handle events like joing or leaving chat
    public class DirectConversation
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime StartDateTime { get; set; }
        public User FirstMember { get; set; }
        public User SecondMember { get; set; }
        public IEnumerable<Message> Messages { get; set; }
    }
}
