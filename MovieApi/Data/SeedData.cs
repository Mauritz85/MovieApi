using Bogus;
using Microsoft.EntityFrameworkCore;
using MovieApi.Models.Entities;

namespace MovieApi.Data;

public class SeedData
{
    private static Faker faker = new Faker("en");

    internal static async Task InitAsync(MovieDbContext context)
    {
        if (await context.Movies.AnyAsync()) return;
       
        int amountOfActors = 20;
        var allActors = GenerateActors(amountOfActors);

        //Generera filmer och koppla in skådisar från listan
        int amountOfMovies = 10;
        var movies = GenerateMovies(amountOfMovies, allActors);

        await context.Actors.AddRangeAsync(allActors); // Lägg till actors separat om de inte sätts som "Cascade"
        await context.Movies.AddRangeAsync(movies);
        await context.SaveChangesAsync();
    }

    private static IEnumerable<Movie> GenerateMovies(int numberOfMovies, List<Actor> allActors)
    {
        var movies = new List<Movie>();

        for (int i = 0; i < numberOfMovies; i++)
        {
            var movie = new Movie
            {
                Title = faker.Lorem.Sentence(3).TrimEnd('.'),
                Year = faker.Date.Past(30).Year,
                Genre = faker.PickRandom("Action", "Drama", "Comedy", "Horror", "Sci-Fi", "Romance"),
                DurationInMinutes = faker.Random.Int(80, 180),

                MovieDetails = new MovieDetails
                {
                    Synopsis = faker.Lorem.Paragraph(),
                    Language = faker.PickRandom("English", "French", "Spanish", "Swedish"),
                    Budget = Math.Round(faker.Random.Decimal(100_000, 200_000_000), 2)
                },

                Reviews = GenerateReviews(faker.Random.Int(1, 5)),

                // Välj slumpmässiga skådespelare från den globala listan
                Actors = allActors
                    .OrderBy(_ => Guid.NewGuid()) // Shuffle
                    .Take(faker.Random.Int(2, 5)) // Ta 2–5 skådisar
                    .ToList()
            };

            movies.Add(movie);
        }

        return movies;
    }

    private static List<Review> GenerateReviews(int count)
    {
        return new Faker<Review>()
            .RuleFor(r => r.ReviewerName, f => f.Name.FullName())
            .RuleFor(r => r.Comment, f => f.Lorem.Sentence())
            .RuleFor(r => r.Rating, f => f.Random.Int(1, 5))
            .Generate(count);
    }

    private static List<Actor> GenerateActors(int count)
    {
        return new Faker<Actor>()
            .RuleFor(a => a.Name, f => f.Name.FullName())
            .RuleFor(a => a.BirthYear, f => f.Date.Past(60, DateTime.Today.AddYears(-20)).Year)
            .Generate(count);
    }
}
