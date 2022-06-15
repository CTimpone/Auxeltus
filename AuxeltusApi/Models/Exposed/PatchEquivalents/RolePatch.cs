using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Auxeltus.Api.Models
{
    public class RolePatch: PatchBase, IValidatableObject
    {
        [MinLength(2, ErrorMessage = "Title must be at least two (2) characters long.")]
        [MaxLength(100, ErrorMessage = "Title must not exceed one hundred (100) characters.")]
        public string Title
        {
            get
            {
                return ObtainValue<string>(nameof(Title));
            }
            set
            {
                SpecifyProperty(nameof(Title), value);
            }
        }

        [Range(0, int.MaxValue, ErrorMessage = "Tier must be a non-negative integer.")]
        public int? Tier
        {
            get
            {
                return ObtainValue<int?>(nameof(Tier));
            }
            set
            {
                SpecifyProperty(nameof(Tier), value);
            }
        }

        [Range(0, int.MaxValue, ErrorMessage = "MaximumSalary must be a non-negative integer.")]
        public int? MaximumSalary
        {
            get
            {
                return ObtainValue<int?>(nameof(MaximumSalary));
            }
            set
            {
                SpecifyProperty(nameof(MaximumSalary), value);
            }
        }

        [Range(0, int.MaxValue, ErrorMessage = "MinimumSalary must be a non-negative integer.")]
        public int? MinimumSalary
        {
            get
            {
                return ObtainValue<int?>(nameof(MinimumSalary));
            }
            set
            {
                SpecifyProperty(nameof(MinimumSalary), value);
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Title) && PropertySpecified(nameof(Title)))
            {
                yield return new ValidationResult(
                    $"The {nameof(Title)} field must not be null if submitted.",
                    new[] { nameof(Title) });
            }

            if (Tier == null && PropertySpecified(nameof(Tier)))
            {
                yield return new ValidationResult(
                    $"The {nameof(Tier)} field must not be null if submitted.",
                    new[] { nameof(Tier) });
            }

            if (MaximumSalary == null && PropertySpecified(nameof(MaximumSalary)))
            {
                yield return new ValidationResult(
                    $"The {nameof(MaximumSalary)} field must not be null if submitted.",
                    new[] { nameof(MaximumSalary) });
            }

            if (MinimumSalary == null && PropertySpecified(nameof(MinimumSalary)))
            {
                yield return new ValidationResult(
                    $"The {nameof(MinimumSalary)} field must not be null if submitted.",
                    new[] { nameof(MinimumSalary) });
            }
        }

    }
}
