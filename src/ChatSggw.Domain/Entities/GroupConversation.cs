using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatSggw.Domain.Entities
{
    //could be inherithed from dirct as a base
    public class GroupConversation
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime StartDateTime { get; set; }
        public IEnumerable<User> Members { get; set; }
        public IEnumerable<Message> Messages { get; set; }
    }
}
