﻿using MovieApi.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MovieApi.Models.DTOs;

public class CreateMovieDto : IValidatableObject
{
    [Required]
    [StringLength(60, MinimumLength = 1, ErrorMessage = "Title length must be between 1 and 60 characters.")]
    public string Title { get; init; } = string.Empty;

    [Required]
    [Range(1900, 2100, ErrorMessage = "Year must be between 1900 and 2100.")]
    public int Year { get; init; }

    [Required]
    public string Genre { get; init; } = string.Empty;

    [Required]
    [Range(60, 500, ErrorMessage = "Duration must be between 60 and 500 minutes.")]
    public int DurationInMinutes { get; init; }

    [Required]
    public CreateMovieDetailsDto MovieDetails { get; init; } = null!;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var allowedGenres = new[] { "Action", "Drama", "Comedy", "Horror" };

        if (!allowedGenres.Contains(Genre))
        {
            yield return new ValidationResult(
                $"Genre must be one of: {string.Join(", ", allowedGenres)}",
                new[] { nameof(Genre) });
        }
    }
}
