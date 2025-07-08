using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieApi.Models.Entities;

namespace MovieApi.Data
{
    public class Context : DbContext
    {
        public Context (DbContextOptions<Context> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieDetails>()
                .Property(m => m.Budget)
                .HasPrecision(18, 2);  
        }

        public DbSet<MovieApi.Models.Entities.Movie> Movie { get; set; } = default!;
    }
}
