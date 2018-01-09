using System.Data.Entity.Spatial;
using ChatSggw.DataLayer;
using ChatSggw.DataLayer.IdentityModels;
using ChatSggw.Domain;
using ChatSggw.Domain.Commands.User;
using Neat.CQRSLite.Contract.Commands;

namespace ChatSggw.Services.Commands.User
{
    public class PingUserLocationCommandHandler : ICommandHandler<PingUserLocationCommand>
    {
        private readonly CoreDbContext _db;

        public PingUserLocationCommandHandler(CoreDbContext db)
        {
            _db = db;
        }

        public void Execute(PingUserLocationCommand command)
        {
            var user = _db.Users.Find(command.UserId);
            user.PingUser(command.Location);
            _db.SaveChanges();
        }
    }
}