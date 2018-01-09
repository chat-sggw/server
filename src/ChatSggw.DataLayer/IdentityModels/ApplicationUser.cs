using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Security.Claims;
using System.Threading.Tasks;
using ChatSggw.Domain.Entities.User;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ChatSggw.DataLayer.IdentityModels
{
    public sealed class ApplicationUser : IdentityUser<Guid, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public ApplicationUser()
        {
            Id = Guid.NewGuid();
            DirectConversations = new List<UserDirectConversation>();
        }


        public bool IsLocalized => CurrentPosition != null;
        public DbGeography CurrentPosition { get; set; }

        public ICollection<UserDirectConversation> DirectConversations { get; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, Guid> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}