using System;
using Neat.CQRSLite.Contract.Commands;

namespace ChatSggw.Domain.Commands.FriendsPair
{
    public class BanFriendCommand : ICommand
    {
        public Guid UserId { get; set; }
        public Guid FriendId { get; set; }
    }
}