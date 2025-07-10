using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Models.DTOs;
using MovieApi.Models.Entities;
using System.Reflection;

namespace MovieApi.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieDbContext _context;

        public MoviesController(MovieDbContext context)
        {
            _context = context;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovie()
        {
            return await _context.Movies.ToListAsync();
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDto>> GetMovie(int id)
        {
            var movie = await _context.Movies
            .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            var movieDto = new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year,
                Genre = movie.Genre,
                DurationInMinutes = movie.DurationInMinutes,     
            };

            return Ok(movieDto);
        }

        // GET: api/Movies/details/5
        [HttpGet("details/{id}")]
        public async Task<ActionResult<MovieWithDetailsDto>> GetMovieDetails(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.MovieDetails)
                .Include(m => m.Reviews)
                .Include(m => m.Actors)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
                return NotFound();

            var movieWithDetailsDto = new MovieWithDetailsDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year,
                Genre = movie.Genre,
                DurationInMinutes = movie.DurationInMinutes,
                MovieDetailsDto = new MovieDetailsDto
                {
                    Synopsis = movie.MovieDetails.Synopsis,
                    Language = movie.MovieDetails.Language,
                    Budget = movie.MovieDetails.Budget
                },
                Reviews = movie.Reviews.Select(r => new ReviewDto
                {
                    Reviewer = r.ReviewerName,
                    Rating = r.Rating,
                    Comment = r.Comment
                }).ToList(),
                Actors = movie.Actors.Select(a => new ActorDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    BirthYear = a.BirthYear
                }).ToList()
            };

            return Ok(movieWithDetailsDto);
        }

        // GET: api/Movies/5/reviews
        [HttpGet("{id}/reviews")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetMovieReviews(int id)
        {
            var movie = await _context.Movies
            .Include(m => m.Reviews)
            .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            var reviews = movie.Reviews.Select(r => new ReviewDto
            {
                Id=r.Id,
                Reviewer = r.ReviewerName,
                Rating = r.Rating,
                Comment = r.Comment
            }).ToList();

            return Ok(reviews);
        }

        // PUT: api/Movies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, UpdateMovieDto updateDto)
        {
            var movie = await _context.Movies
                .Include(m => m.MovieDetails)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            movie.Title = updateDto.Title;
            movie.Year = updateDto.Year;
            movie.Genre = updateDto.Genre;
            movie.DurationInMinutes = updateDto.DurationInMinutes;
            if (movie.MovieDetails is null)
            {
                movie.MovieDetails = new MovieDetails
                {
                    Synopsis = updateDto.MovieDetails.Synopsis,
                    Language = updateDto.MovieDetails.Language,
                    Budget = updateDto.MovieDetails.Budget
                };
            }
            else
            {
                movie.MovieDetails.Synopsis = updateDto.MovieDetails.Synopsis;
                movie.MovieDetails.Language = updateDto.MovieDetails.Language;
                movie.MovieDetails.Budget = updateDto.MovieDetails.Budget;
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(MovieWithDetailsDto), new { id = movie.Id });
        }


        // POST: api/Movies
        [HttpPost]
        public async Task<ActionResult<MovieDto>> PostMovie(CreateMovieDto createDto)
        {
            var movie = new Movie
            {
                Title = createDto.Title,
                Year = createDto.Year,
                Genre = createDto.Genre,
                DurationInMinutes = createDto.DurationInMinutes,
                MovieDetails = new MovieDetails
                {
                    Synopsis = createDto.MovieDetails.Synopsis,
                    Language = createDto.MovieDetails.Language,
                    Budget = createDto.MovieDetails.Budget
                }
            };

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            var movieWithDetailsDto = new MovieWithDetailsDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year,
                Genre = movie.Genre,
                DurationInMinutes = movie.DurationInMinutes,
                MovieDetailsDto = new MovieDetailsDto
                {
                    Synopsis = movie.MovieDetails.Synopsis,
                    Language = movie.MovieDetails.Language,
                    Budget = movie.MovieDetails.Budget
                }

            };

            return CreatedAtAction(nameof(GetMovieDetails), new { id = movie.Id }, movieWithDetailsDto);
        }

        // POST: /api/movies/{movieId}/actors/{actorId}
        [HttpPost("{movieId}/actors/{actorId}")]
        public async Task<ActionResult> AddActorToMovie(int movieId, int actorId)
        {
            var movie = await _context.Movies
                .Include(m => m.Actors)
                .FirstOrDefaultAsync(m => m.Id == movieId);

            if (movie == null)
            {
               return NotFound($"Movie with ID {movieId} not found");
            }

            var actor = await _context.Actors.FindAsync(actorId);

            if (actor == null)
            {
                return NotFound($" with ID {actorId} not found");
            }

            if (movie.Actors.Any(a => a.Id == actorId))
            {
                return NotFound($"This Actor is already assigned to this movie");
            }

            movie.Actors.Add(actor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMovieDetails), new { id = movie.Id });
        }


        // POST: /api/movies/{movieId}/reviews
        [HttpPost("{movieId}/reviews/")]
        public async Task<ActionResult> PostReview(int movieId, CreateReviewDto createDto)
        {
            var movie = await _context.Movies
                .Include(m => m.Reviews)
                .FirstOrDefaultAsync(m => m.Id == movieId);

            if (movie == null)
            {
                return NotFound($"Movie with ID {movieId} not found");
            }

            var review = new Review
            {
                ReviewerName = createDto.ReviewerName,
                Rating = createDto.Rating,
                Comment = createDto.Comment
            };
 

            movie.Reviews.Add(review);
            await _context.SaveChangesAsync();

            var reviewDto = new ReviewDto
            {
                Reviewer = review.ReviewerName,
                Rating = review.Rating,
                Comment = review.Comment
            };

            return CreatedAtAction(nameof(GetMovieDetails), new { id = movie.Id }, reviewDto );
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.MovieDetails)
                .Include(m => m.Reviews)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
