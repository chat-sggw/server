using System.Data.Entity.Spatial;
using System.Globalization;
using ChatSggw.Domain.Structs;

namespace ChatSggw.Domain
{
    public static class DbGeographyHelper
    {
        public static DbGeography ConvertLatLonToDbGeography(double longitude, double latitude)
        {
            var point = string.Format("POINT({1} {0})",
                latitude.ToString(CultureInfo.InvariantCulture),
                longitude.ToString(CultureInfo.InvariantCulture));
            return DbGeography.FromText(point);
        }

        public static DbGeography ConvertLatLonToDbGeography(this GeoInformation geoLocation)
        {
            return ConvertLatLonToDbGeography(geoLocation.Longitude, geoLocation.Latitude);
        }
    }
}