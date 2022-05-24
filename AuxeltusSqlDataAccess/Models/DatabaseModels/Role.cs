using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Auxeltus.AccessLayer.Sql
{
    public class Role
    {
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public int? Tier { get; set; }

        [Required]
        public int? MinimumSalary { get; set; }

        [Required]
        public int? MaximumSalary { get; set; }

        public List<Job>? Jobs { get; set; }

        internal void Mutate(Role updatedRole)
        {
            Title = updatedRole.Title ?? Title;
            Tier = updatedRole.Tier ?? Tier;
            MinimumSalary = updatedRole.MinimumSalary ?? MinimumSalary;
            MaximumSalary = updatedRole.MaximumSalary ?? MaximumSalary;

        }
    }
}
