namespace MovieApi.Models.DTOs;
using System.ComponentModel.DataAnnotations;

    public class CreateReviewDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Reviewer name must be between 2 and 100 characters.")]
        public string ReviewerName { get; set; } = string.Empty;

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

    [StringLength(1000, MinimumLength = 3, ErrorMessage = "Comment can't be longer than 1000 and atleast 3 characters.")]
    public string Comment { get; set; } = string.Empty;
    }

