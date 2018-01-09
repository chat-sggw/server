using System;

namespace ChatSggw.Domain.DTO.FriendsPair
{
    public class FriendsPairDTO
    {
        //dzieki zachowaniu tego pierwszego i drugiego (oraz obsluzeniu, zeby nie wstawial
        //w druga strone) mozemy wyluskac, kto kogo dodal, gdyby przyszlo do glowy kiedys :)
        public Guid FirstUserId { get; set; }
        public Guid SecondUserId { get; set; }
        public DateTime DateTimeStamp { get; set; }
    }
}
