using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.Linq;
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
    public class RemoveMemberFromConversationCommandHandler : ICommandHandler<RemoveMemberFromConversationCommand>
    {
        private readonly CoreDbContext _db;

        public RemoveMemberFromConversationCommandHandler(CoreDbContext db)
        {
            _db = db;
        }

        public void Execute(RemoveMemberFromConversationCommand command)
        {            
            var conversation = _db.Conversations.Include(x => x.Members)
                .SingleOrDefault(x => x.Id == command.ConversationId);

            conversation.RemoveMember(command.MemberId);            
            _db.SaveChanges();
        }
    }
}