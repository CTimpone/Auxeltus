using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Auxeltus.AccessLayer.Sql
{
    public class Location
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public double? Latitude { get; set; }

        [Required]
        public double? Longitude { get; set; }

        public List<Job>? Jobs { get; set; }
    }
}
