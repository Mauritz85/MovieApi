using System.ComponentModel.DataAnnotations;

public class CreateMovieDetailsDto: IValidatableObject
{
    [Required]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Synopsis needs to be atleast 3 characters.")]
    public string Synopsis { get; init; } = null!;

    [Required]
    public string Language { get; init; } = null!;

    [Range(10000, 1_000_000_000, ErrorMessage ="Budget must be between 10 000 and 1 000 000 000")]
    public decimal Budget { get; init; }
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
