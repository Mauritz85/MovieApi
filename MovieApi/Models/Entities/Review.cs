namespace MovieApi.Models.Entities;

public class Review
{
    public int Id { get; set; }
    public string ReviewerName { get; set; } = null!;
    public string Comment { get; set;} = null!;
    public int Rating { get; set; }

    // foreign key
    public int MovieId { get; set; }
    
    //navigation property
    public Movie Movie { get; set; } = null!;
}
