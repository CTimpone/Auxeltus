using System.ComponentModel.DataAnnotations;

namespace Auxeltus.Api.Models
{
    public class Role
    {
        public int? Id { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int? Tier { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "MaximumSalary must be a non-negative integer.")]
        public int? MaximumSalary { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "MinimumSalary must be a non-negative integer.")]
        public int? MinimumSalary { get; set; }
    }
}
