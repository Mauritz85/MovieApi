using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieApi.Models.Entities;

public class MovieDetails
{
    [Key, ForeignKey("Movie")]
    public int Id { get; set; }
    public string Synopsis { get; set; } = null!;
    public string Language { get; set; } = null!;
    public decimal Budget { get; set; }

    //Navigation property
    public Movie? Movie { get; set; }

}
