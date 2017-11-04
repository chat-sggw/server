using System;
using ChatSggw.DataLayer.IdentityModels;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ChatSggw.DataLayer
{
    public class CoreDbContext : IdentityDbContext<ApplicationUser, CustomRole, Guid,
        CustomUserLogin, CustomUserRole, CustomUserClaim>
    {

    }
}