using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Auxeltus.AccessLayer.Sql
{
    public class Location
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        [Required]
        public double? Latitude { get; set; }

        [Required]
        public double? Longitude { get; set; }

        public List<Job>? Jobs { get; set; }

        internal void Mutate(Location updatedLocation)
        {
            Name = updatedLocation.Name ?? Name;
            Latitude = updatedLocation.Latitude ?? Latitude;
            Longitude = updatedLocation.Longitude ?? Longitude;
        }

        public double DistanceBetween(Location otherLocation)
        {
            if (Latitude == null || Longitude == null || otherLocation.Latitude == null || otherLocation.Longitude == null)
            {
                throw new ArgumentException("Invalid coordinates for distance calculation");
            }

            double oneDegree = Math.PI / 180.0;

            double latRadians = Latitude.Value * oneDegree;
            double longRadians = Longitude.Value * oneDegree;
            double otherLatRadians = otherLocation.Latitude.Value * oneDegree;
            double longDiffRadians = otherLocation.Longitude.Value * oneDegree - longRadians;
            double a = Math.Pow(Math.Sin((otherLatRadians - latRadians) / 2.0), 2.0)
                + Math.Cos(longRadians) * Math.Cos(otherLatRadians) * Math.Pow(Math.Sin(longDiffRadians / 2.0), 2.0);

            return 6376.50 * (2.0 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1.0 - a)));

        }
    }
}