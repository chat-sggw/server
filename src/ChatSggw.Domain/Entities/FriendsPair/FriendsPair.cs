using System;

namespace ChatSggw.Domain.Entities.FriendsPair
{
    public class FriendsPair
    {
        public Guid FirstUserId { get; set; }
        public Guid SecondUserId { get; set; }
        public DateTime DateTimeStamp { get; set; }

        public FriendsPair(Guid firstUserId, Guid secondUserId)
        {
            FirstUserId = firstUserId;
            SecondUserId = secondUserId;
            DateTimeStamp = DateTime.Now;
        }
    }
}
