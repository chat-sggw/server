using System;
using Neat.CQRSLite.Contract.Commands;

namespace ChatSggw.Domain.Commands.FriendsPair
{
    public class AddRemoveFriendCommand : ICommand
    {
        public Guid FirstUserId { get; set; }
        public Guid SecondUserId { get; set; }
    }
}