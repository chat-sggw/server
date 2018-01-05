using System;
using Neat.CQRSLite.Contract.Commands;

namespace ChatSggw.Domain.Commands.User
{
    public class AddFriendCommand : ICommand
    {
        public Guid FirstUserId { get; set; }
        public Guid SecondUserId { get; set; }
    }
}