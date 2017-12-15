using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatSggw.Domain.DTO
{
    public class ConversationModel
    {
        public Guid Id { get; set; }
        public DateTime StartDateTime { get; set; }
        public IEnumerable<MessageModel> Messages { get; set; }
    }
}
