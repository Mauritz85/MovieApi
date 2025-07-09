using System.ComponentModel.DataAnnotations;

public class CreateMovieDetailsDto
{
    [Required]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Synopsis needs to be atleast 3 characters.")]
    public string Synopsis { get; set; } = null!;

    [Required]
    public string Language { get; set; } = null!;

    [Range(10000, 1_000_000_000)]
    public decimal Budget { get; set; }
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var allowedLanguages = new[] {"English", "French", "Spanish", "Swedish"};

        if (!allowedLanguages.Contains(Language))
        {
            yield return new ValidationResult(
                $"Genre must be one of: {string.Join(", ", allowedLanguages)}",
                new[] { nameof(Language) });
        }
    }
}
