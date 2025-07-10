namespace MovieApi.Models.DTOs;

public record MovieDto
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public int Year { get; init; }
    public string Genre { get; init; } = string.Empty;
    public int DurationInMinutes { get; init; }

}
