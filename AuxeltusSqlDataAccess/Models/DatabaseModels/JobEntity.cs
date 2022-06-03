using System.ComponentModel.DataAnnotations;

namespace Auxeltus.AccessLayer.Sql
{

    /// <summary>
    /// Class <c>Job</c> is a representation of a working position within the company.
    /// It has a foreign-key relationship with the <c>Role</c> model.
    /// It has an optional one-to relationship with the <c>Location</c> model.
    /// If there is no populated <c>Location</c>, then the <c>Job</c> must be marked as Remote.
    /// A <c>Job</c> can be archived to keep the record while removing it from standard access patterns.
    /// It serves as an EF Core model for the purposes of the Auxeltus data structure.
    /// </summary>
    public class JobEntity
    {
        public int Id { get; set; }

        [Required]
        [StringLength(2000)]
        public string? Description { get; set; }

        public double? Salary { get; set; }

        public SalaryType? SalaryType { get; set; }

        [Required]
        public EmployeeType? EmployeeType { get; set; }

        public int? EmployeeId { get; set; }

        [Required]
        public int? ReportingEmployeeId { get; set; }

        [Required]
        public bool? Archived { get; set; }

        [Required]
        public int RoleId { get; set; }
        public RoleEntity Role { get; set; }

        public bool? Remote { get; set; }

        public int? LocationId { get; set; }
        public LocationEntity? Location { get; set; }

        /// <summary>
        /// Modify the relevant underlying values on <c>Job</c> parameters to facilitate EF Core updates.
        /// </summary>
        internal void Mutate(JobEntity otherJob)
        {
            Description = otherJob.Description ?? Description;
            Salary = otherJob.Salary ?? Salary;
            SalaryType = otherJob.SalaryType ?? SalaryType;
            EmployeeType = otherJob.EmployeeType ?? EmployeeType;
            EmployeeId = otherJob.EmployeeId ?? EmployeeId;
            ReportingEmployeeId = otherJob.ReportingEmployeeId ?? ReportingEmployeeId;
            Archived = otherJob.Archived ?? Archived;

            if (otherJob.Remote == true)
            {
                Location = null;
                LocationId = null;
                Remote = true;
            } else if (otherJob.Location != null || (otherJob.LocationId != null && otherJob.LocationId != 0))
            {
                Location = otherJob.Location;
                LocationId = otherJob.LocationId;

                Remote = otherJob.Remote;
            }

            if (otherJob.Role != null || otherJob.RoleId != 0)
            {
                Role = otherJob.Role;
                RoleId = otherJob.RoleId == 0 ? otherJob.Role.Id : otherJob.RoleId;
            }
        }
    }
}
