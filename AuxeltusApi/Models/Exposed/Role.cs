using System.ComponentModel.DataAnnotations;

namespace Auxeltus.Api.Models
{
    public class Role
    {
        public int? Id { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Title must be at least two (2) characters long.")]
        [MaxLength(100, ErrorMessage = "Title must not exceed one hundred (100) characters.")]
        public string Title { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Tier must be a non-negative integer.")]
        public int? Tier { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "MaximumSalary must be a non-negative integer.")]
        public int? MaximumSalary { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "MinimumSalary must be a non-negative integer.")]
        public int? MinimumSalary { get; set; }
    }
}