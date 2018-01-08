using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieLibrary.Data.Models;

namespace MovieLibrary.Data
{
    public class MovieLibraryDbContext : IdentityDbContext<User>
    {
        public DbSet<Category> Categories { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<MovieCategory> MovieCategories { get; set; }
        public DbSet<MovieActior> MovieActiors { get; set; }
        public DbSet<MovieWriter> MovieWriters { get; set; }
        public DbSet<MovieDirector> MovieDirectors { get; set; }

        public DbSet<Person> Persons { get; set; }

        public DbSet<Vote> Votes { get; set; }

        public DbSet<User> Users { get; set; }

        public MovieLibraryDbContext(DbContextOptions<MovieLibraryDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MovieCategory>().HasKey(k => new { k.MovieId, k.CategoryId });
            builder.Entity<MovieDirector>().HasKey(k => new { k.MovieId, k.DirectorId });
            builder.Entity<MovieWriter>().HasKey(k => new { k.MovieId, k.WriterId });
            builder.Entity<MovieActior>().HasKey(k => new { k.MovieId, k.ActiorId });
            builder.Entity<Vote>().HasKey(k => new { k.MovieId, k.UserId });

            builder.Entity<Movie>().HasMany(m => m.Categories).WithOne(c => c.Movie).HasForeignKey(k => k.MovieId);
            builder.Entity<Category>().HasMany(m => m.Movies).WithOne(c => c.Category).HasForeignKey(k => k.CategoryId);


            builder.Entity<Movie>().HasMany(m => m.Actiors).WithOne(c => c.Movie).HasForeignKey(k => k.MovieId);
            builder.Entity<Person>().HasMany(m => m.MoviesActior).WithOne(c => c.Actior).HasForeignKey(k => k.ActiorId);

            builder.Entity<Movie>().HasMany(m => m.Director).WithOne(c => c.Movie).HasForeignKey(k => k.MovieId);
            builder.Entity<Person>().HasMany(m => m.MoviesDirector).WithOne(c => c.Director).HasForeignKey(k => k.DirectorId);

            builder.Entity<Movie>().HasMany(m => m.Writers).WithOne(c => c.Movie).HasForeignKey(k => k.MovieId);
            builder.Entity<Person>().HasMany(m => m.MoviesWriter).WithOne(c => c.Writer).HasForeignKey(k => k.WriterId);


            builder.Entity<Movie>().HasMany(m => m.Comments).WithOne(c => c.Movie).HasForeignKey(k => k.MovieId);
            builder.Entity<User>().HasMany(u => u.Comments).WithOne(c => c.Author).HasForeignKey(k => k.AuthorId);

            builder.Entity<Movie>().HasMany(m => m.Votes).WithOne(d => d.Movie).HasForeignKey(k => k.MovieId);
            builder.Entity<User>().HasMany(m => m.Votes).WithOne(d => d.User).HasForeignKey(k => k.UserId);

            builder.Entity<User>().HasMany(m => m.UploadedMovies).WithOne(d => d.Uploader).HasForeignKey(k => k.UploaderId);

            base.OnModelCreating(builder);
        }
    }
}
