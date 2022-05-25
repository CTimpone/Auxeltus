using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Auxeltus.AccessLayer.Sql
{
    /// <summary>
    /// Class <c>Role</c> is a representation of a position somewhere on Earth.
    /// It serves as an EF Core model for the purposes of the Auxeltus data structure.
    /// </summary>
    public class Role
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Title { get; set; }

        [Required]
        public int? Tier { get; set; }

        [Required]
        public int? MinimumSalary { get; set; }

        [Required]
        public int? MaximumSalary { get; set; }

        public List<Job>? Jobs { get; set; }

        /// <summary>
        /// Modify the relevant underlying values on <c>Role</c> parameters to facilitate EF Core updates.
        /// </summary>
        internal void Mutate(Role updatedRole)
        {
            Title = updatedRole.Title ?? Title;
            Tier = updatedRole.Tier ?? Tier;
            MinimumSalary = updatedRole.MinimumSalary ?? MinimumSalary;
            MaximumSalary = updatedRole.MaximumSalary ?? MaximumSalary;

        }
    }
}
