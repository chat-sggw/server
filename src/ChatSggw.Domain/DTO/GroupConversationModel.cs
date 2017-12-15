using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatSggw.Domain.DTO
{
    public class GroupConversationModel
    {
        public IEnumerable<UserModel> Members { get; set; }
    }
}
