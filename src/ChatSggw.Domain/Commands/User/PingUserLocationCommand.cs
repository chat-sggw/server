using System;
using ChatSggw.Domain.Structs;
using Neat.CQRSLite.Contract.Commands;

namespace ChatSggw.Domain.Commands.User
{
    public class PingUserLocationCommand : ICommand
    {
        public Guid UserId { set; get; }
        public GeoInformation Location { get; set; }
    }
}