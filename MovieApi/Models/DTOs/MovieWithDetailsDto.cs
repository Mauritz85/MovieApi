using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models.DTOs
{
    public record MovieWithDetailsDto: MovieDto
    {
        [Required]
        required public MovieDetailsDto MovieDetailsDto { get; init; } = null!;
        public IEnumerable<ReviewDto>? Reviews { get; init; } 
        public IEnumerable<ActorDto>? Actors { get; init; } 
    }
}
