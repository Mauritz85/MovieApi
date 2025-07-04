using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieApi.Models.Entities;

public class MovieDetails
{
    [Key]
    [ForeignKey("Movie")]
    public int Id { get; set; }
    public string Synopsis { get; set; } = null!;
    public int Language { get; set; }
    public decimal Budget { get; set; }
    public Movie Movie { get; set; } = null!;

}
