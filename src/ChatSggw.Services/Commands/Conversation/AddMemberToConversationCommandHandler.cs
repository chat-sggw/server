using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Reflection;
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
    public class AddMemberToConversationCommandHandler : ICommandHandler<AddMemberToConversationCommand>
    {
        private readonly CoreDbContext _db;

        public AddMemberToConversationCommandHandler(CoreDbContext db)
        {
            _db = db;
        }

        public void Execute(AddMemberToConversationCommand command)
        {
            var member = _db.Users.Find(command.MemberId);
            var conversation = _db.Conversations.Include(x => x.Members)
                .SingleOrDefault(x => x.Id == command.ConversationId);
            
            if (conversation == null)
            {
                throw new Exception("The conversation doesn't exist!");
            }
            conversation.AddMember(member.Id);

            _db.SaveChanges();
        }
    }
}