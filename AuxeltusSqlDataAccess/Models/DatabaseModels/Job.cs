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

        public void Mutate(Job otherJob)
        {
            Description = otherJob.Description ?? Description;
            Salary = otherJob.Salary ?? Salary;
            EmployeeType = otherJob.EmployeeType ?? EmployeeType;
            EmployeeId = otherJob.EmployeeId ?? EmployeeId;
            ReportingEmployeeId = otherJob.ReportingEmployeeId ?? ReportingEmployeeId;

            if (otherJob.Remote == true)
            {
                Location = null;
                LocationId = null;
                Remote = true;
            } else if (otherJob.LocationId != null)
            {
                Location = otherJob.Location;
                LocationId = otherJob.LocationId;

                Remote = otherJob.Remote;
            } 
        }
    }
}
