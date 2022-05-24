using System.ComponentModel.DataAnnotations;

namespace Auxeltus.AccessLayer.Sql
{
    public class Job
    {
        public int Id { get; set; }

        [Required]
        public string? Description { get; set; }

        public double? Salary { get; set; }

        [Required]
        public EmployeeType? EmployeeType { get; set; }

        public int? EmployeeId { get; set; }

        [Required]
        public int? ReportingEmployeeId { get; set; }

        [Required]
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public bool? Remote { get; set; }

        public int? LocationId { get; set; }
        public Location? Location { get; set; }
    }
}
