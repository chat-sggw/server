using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using ChatSggw.DataLayer;
using ChatSggw.DataLayer.IdentityModels;
using ChatSggw.Domain;
using ChatSggw.Domain.Commands.Conversation;
using ChatSggw.Domain.Commands.User;
using ChatSggw.Domain.Entities.Conversation;
using ChatSggw.Domain.Entities.User;
using Neat.CQRSLite.Contract.Commands;

namespace ChatSggw.Services.Commands.User
{
    public class CreateGroupConversationCommandHandler : ICommandHandler<CreateGroupConversationCommand>
    {
        private readonly CoreDbContext _db;

        public CreateGroupConversationCommandHandler(CoreDbContext db)
        {
            _db = db;
        }

        public void Execute(CreateGroupConversationCommand command)
        {
            var members = new List<Guid>();

            var conversation = Conversation.CreateGroupConversation(members);
            _db.Conversations.Add(conversation);
            
            _db.SaveChanges();
        }
    }
}