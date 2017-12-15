using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatSggw.Domain.DTO
{
    public class DirectConversationModel : ConversationModel
    {
        public UserModel FirstMember { get; set; }
        public UserModel SecondMember { get; set; }
    }
}
