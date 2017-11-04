using System;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ChatSggw.DataLayer.IdentityModels
{
    public class CustomRole : IdentityRole<Guid, CustomUserRole>
    {
        public CustomRole()
        {
            Id = Guid.NewGuid();
        }
    }
}