using System;

namespace ChatSggw.Domain.Structs
{
    public struct GeoInformation
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }


        public double GetDistance(GeoInformation other)
        {
            const double p = 0.017453292519943295; // Math.PI / 180
            var a = 0.5 - Math.Cos((other.Latitude - Latitude) * p) / 2 +
                    Math.Cos(Latitude * p) * Math.Cos(other.Latitude * p) *
                    (1 - Math.Cos((other.Longitude - Longitude) * p)) / 2;

            return 12742 * Math.Asin(Math.Sqrt(a)); // 2 * R; R = 6371 km
        }
    }
}
