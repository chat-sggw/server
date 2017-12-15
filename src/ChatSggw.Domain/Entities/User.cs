using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatSggw.Domain.Structs;

namespace ChatSggw.Domain.Entities
{
    //sample class for user because I need basic info like UserName now 
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocalized { get; set; }
        public GeoInformation? CurrentPosition { get; set; }
    }
}
