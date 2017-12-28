using System;
using System.Data.Entity;
using ChatSggw.DataLayer.IdentityModels;
using ChatSggw.Domain.Entities.Conversation;
using ChatSggw.Domain.Entities.User;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ChatSggw.DataLayer
{
    public class CoreDbContext : IdentityDbContext<ApplicationUser, CustomRole, Guid,
        CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public CoreDbContext(Settings settings) : base(settings.ConnectionString)
        {
        }

        public DbSet<Conversation> Conversations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Conversation mapping
            modelBuilder.Entity<Conversation>()
                .HasKey(c => c.Id);
            modelBuilder.Entity<Conversation>()
                .HasMany(c => c.Messages)
                .WithRequired()
                .HasForeignKey(m => m.ConversationId)
                .WillCascadeOnDelete();
            modelBuilder.Entity<Conversation>()
                .HasMany(c => c.Members)
                .WithRequired()
                .WillCascadeOnDelete();
            modelBuilder.Entity<Message>().HasKey(m => new {m.ConversationId, m.Id});
            modelBuilder.Entity<ConversationMember>().HasKey(m => new {m.ConversationId, m.UserId});

            //UserMapping
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.DirectConversations)
                .WithRequired()
                .WillCascadeOnDelete();

            modelBuilder.Entity<UserDirectConversation>()
                .HasKey(udc => new {udc.UserId, udc.InterlocutorId});

            base.OnModelCreating(modelBuilder);
        }

        public class Settings
        {
            public string ConnectionString { get; set; }
        }
    }
}