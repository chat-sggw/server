using System;
using System.Security.Policy;
using ChatSggw.Domain.Entities.Conversation;
using Neat.CQRSLite.Contract.Commands;

namespace ChatSggw.Domain.Commands.User
{
    public class SendMessageCommand : ICommand
    {
        public Message Message { get; set; }
    }
}
