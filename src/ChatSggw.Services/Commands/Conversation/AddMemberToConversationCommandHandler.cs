﻿using System;
using System.Data.Entity;
using System.Linq;
using ChatSggw.DataLayer;
using ChatSggw.Domain.Commands.Conversation;
using Neat.CQRSLite.Contract.Commands;

namespace ChatSggw.Services.Commands.Conversation
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

            if (conversation.Members.All(m => m.UserId != command.UserId))
            {
                throw new Exception("Brak uprawnień");
            }

            conversation.AddMember(member.Id);

            _db.SaveChanges();
        }
    }
}