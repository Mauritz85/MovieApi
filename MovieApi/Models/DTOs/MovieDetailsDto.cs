namespace MovieApi.Models.DTOs;

public record MovieDetailsDto
{
    public string Synopsis { get; init; } = string.Empty;
    public string Language { get; init; } = string.Empty;
    public decimal Budget { get; init; }


}
