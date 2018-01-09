using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Security.Claims;
using System.Threading.Tasks;
using ChatSggw.Domain.Structs;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ChatSggw.DataLayer.IdentityModels
{
    public sealed class ApplicationUser : IdentityUser<Guid, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public ApplicationUser()
        {
            Id = Guid.NewGuid();
        }


        public bool IsLocalized => CurrentPositionLongitude.HasValue && CurrentPositionLatitude.HasValue;
        public double? CurrentPositionLongitude {  get; private set; }
        public double? CurrentPositionLatitude {  get; private set; }


        public GeoInformation? CurrentPosition => IsLocalized
            ? new GeoInformation()
            {
                Longitude = CurrentPositionLongitude.Value,
                Latitude = CurrentPositionLatitude.Value
            }
            : (GeoInformation?) null;

        public void PingUser(GeoInformation? geoStampGeoInformation = null)
        {
            if (geoStampGeoInformation.HasValue)
            {
                CurrentPositionLongitude = geoStampGeoInformation.Value.Longitude;
                CurrentPositionLatitude = geoStampGeoInformation.Value.Latitude;
            }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, Guid> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}