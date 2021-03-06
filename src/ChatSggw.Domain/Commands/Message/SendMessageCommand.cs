﻿using System;
using ChatSggw.Domain.Structs;
using Neat.CQRSLite.Contract.Commands;

namespace ChatSggw.Domain.Commands.Message
{
    public class SendMessageCommand : ICommand
    {
        public Guid MessageId { get; set; }
        public Guid UserId { get; set; }
        public Guid ConversationId { get; set; }
        //geostamp brany jest tego co jest zapisane w profilu użytkownika 
        //public GeoInformation GeoStamp { get; set; }
        public string Text { get; set; }
    }
}
