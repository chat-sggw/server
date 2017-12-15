using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatSggw.Domain.Structs;

namespace ChatSggw.Domain.DTO
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocalized { get; set; }
        public GeoInformation? CurrentPosition { get; set; }
    }
}
