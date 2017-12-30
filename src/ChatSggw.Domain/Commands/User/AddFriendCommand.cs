using System;
using Neat.CQRSLite.Contract.Commands;

namespace ChatSggw.Domain.Commands.User
{
    public class AddFriendCommand : ICommand
    {
        public Guid UserId { get; set; }
        public Guid FriendId { get; set; }
    }
}