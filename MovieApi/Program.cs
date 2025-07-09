using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieApi.Data;
using MovieApi.Extensions;

namespace MovieApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            const string CONNECTION_STRING_NAME = "DefaultConnection";
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<MovieDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString(CONNECTION_STRING_NAME) ?? throw new InvalidOperationException("Connection string 'Context' not found.")));

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                await app.SeedDataAsync();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
