using System;
using ChatSggw.DataLayer.IdentityModels;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ChatSggw.DataLayer
{
    public class CoreDbContext : IdentityDbContext<ApplicationUser, CustomRole, Guid,
        CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public CoreDbContext(Settings settings) : base(settings.ConnectionString)
        {
        }

        public class Settings
        {
            public string ConnectionString { get; set; }
        }
    }
}