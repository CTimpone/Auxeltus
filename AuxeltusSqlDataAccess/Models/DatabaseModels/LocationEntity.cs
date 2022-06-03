using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Auxeltus.AccessLayer.Sql
{
    /// <summary>
    /// Class <c>Location</c> is a representation of a position somewhere on Earth.
    /// It serves as an EF Core model for the purposes of the Auxeltus data structure.
    /// </summary>
    public class LocationEntity
    {
        public int Id { get; set; }

        /// <summary>
        /// The identifying name for the <c>Location</c>.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        /// <summary>
        /// The Latitude coordinate for the <c>Location</c>.
        /// It should include the full value (degree, minutes, seconds, etc), as there are no other fields for more granular coordinate population.
        /// </summary>
        [Required]
        public double? Latitude { get; set; }

        /// <summary>
        /// The Longitude coordinate for the <c>Location</c>.
        /// It should include the full value (degree, minutes, seconds, etc), as there are no other fields for more granular coordinate population.
        /// </summary>
        [Required]
        public double? Longitude { get; set; }

        /// <summary>
        /// The full list of <c>Job</c>s that are located at the <c>Location</c>.
        /// Relates to the EF Core data model, in which a <c>Job</c> may have a foreign key pointing to a <c>Location</c>.
        /// </summary>
        public List<JobEntity>? Jobs { get; set; }

        /// <summary>
        /// Modify the relevant underlying values on <c>Location</c> parameters to facilitate EF Core updates.
        /// </summary>
        internal void Mutate(LocationEntity updatedLocation)
        {
            Name = updatedLocation.Name ?? Name;
            Latitude = updatedLocation.Latitude ?? Latitude;
            Longitude = updatedLocation.Longitude ?? Longitude;
        }

        /// <summary>
        /// The <c>DistanceBetween</c> method calculates the distance between two location objects, returning the value in kilometers.
        /// In the event that one or more coordinates is not populated in the locations, an <c>ArgumentException</c> will be thrown.
        /// </summary>
        public double DistanceBetween(LocationEntity otherLocation)
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