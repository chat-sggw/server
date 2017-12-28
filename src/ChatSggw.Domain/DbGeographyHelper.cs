using System.Data.Entity.Spatial;

namespace ChatSggw.Domain
{
    public static class DbGeographyHelper
    {
        public static DbGeography ConvertLatLonToDbGeography(double longitude, double latitude)
        {
            var point = string.Format("POINT({1} {0})", latitude, longitude);
            return DbGeography.FromText(point);
        }
    }
}