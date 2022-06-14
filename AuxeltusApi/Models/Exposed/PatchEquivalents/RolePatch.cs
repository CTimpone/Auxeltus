using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Auxeltus.Api.Models
{
    public class RolePatch: IValidatableObject
    {
        internal bool TitleSpecified;

        internal bool TierSpecified;

        internal bool MaximumSalarySpecified;

        internal bool MinimumSalarySpecified;

        private string _title;
        private int? _tier;
        private int? _maxSalary;
        private int? _minSalary;

        [MinLength(1)]
        [MaxLength(100)]
        [BindProperty]
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                TitleSpecified = true;
                _title = value;
            }
        }

        [Range(0, int.MaxValue)]
        [BindProperty]
        public int? Tier
        {
            get
            {
                return _tier;
            }
            set
            {
                TierSpecified = true;
                _tier = value;
            }
        }

        [Range(0, int.MaxValue, ErrorMessage = "MaximumSalary must be a non-negative integer.")]
        [BindProperty]
        public int? MaximumSalary
        {
            get
            {
                return _maxSalary;
            }
            set
            {
                MaximumSalarySpecified = true;
                _maxSalary = value;
            }
        }

        [Range(0, int.MaxValue, ErrorMessage = "MinimumSalary must be a non-negative integer.")]
        [BindProperty]
        public int? MinimumSalary
        {
            get
            {
                return _minSalary;
            }
            set
            {
                MinimumSalarySpecified = true;
                _minSalary = value;
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Title == null && TitleSpecified)
            {
                yield return new ValidationResult(
                    $"The {nameof(Title)} field must not be null if submitted.",
                    new[] { nameof(Title) });
            }

            if (Tier == null && TierSpecified)
            {
                yield return new ValidationResult(
                    $"The {nameof(Tier)} field must not be null if submitted.",
                    new[] { nameof(Tier) });
            }

            if (MaximumSalary == null && MaximumSalarySpecified)
            {
                yield return new ValidationResult(
                    $"The {nameof(MaximumSalary)} field must not be null if submitted.",
                    new[] { nameof(MaximumSalary) });
            }

            if (MinimumSalary == null && MinimumSalarySpecified)
            {
                yield return new ValidationResult(
                    $"The {nameof(MinimumSalary)} field must not be null if submitted.",
                    new[] { nameof(MinimumSalary) });
            }
        }

    }
}
