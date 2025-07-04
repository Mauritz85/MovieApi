namespace MovieApi.Models.Entities;

public class Review
{
    public int Id { get; set; }
    public string RevierName { get; set; } = null!;
    public int Comment { get; set; }
    public int Rating { get; set;}
}