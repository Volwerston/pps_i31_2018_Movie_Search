using System;
using FilmSearch.Models;
using FilmSearch.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FilmSearch.DAL
{
    public class FilmSearchContext: IdentityDbContext<AppUser>
    {
        public FilmSearchContext(DbContextOptions<FilmSearchContext> options):
            base(options)
        {
        }

        public FilmSearchContext(string connectionString):
            base(new DbContextOptionsBuilder<FilmSearchContext>().UseNpgsql(connectionString).Options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FilmGenre>()
                .HasKey(fg => new {fg.FilmId, fg.GenreId});
            modelBuilder.Entity<FilmGenre>()
                .HasOne(fg => fg.Film)
                .WithMany(f => f.Genres)
                .HasForeignKey(fg => fg.FilmId);
            modelBuilder.Entity<FilmGenre>()
                .HasOne(fg => fg.Genre)
                .WithMany(g => g.Films)
                .HasForeignKey(fg => fg.GenreId);
        }

        public DbSet<File> Files { get; set; }
        
        public DbSet<CommentOpinion> CommentOpinions { get; set; }
        
        public DbSet<Film> Films { get; set; }
        
        public DbSet<FilmRole> FilmRoles { get; set; }
        
        public DbSet<FilmGenre> FilmGenres { get; set; }
        
        public DbSet<FilmPerformance> FilmPerformances { get; set; }
        
        public DbSet<Genre> Genres { get; set; }
        
        public DbSet<Person> Persons { get; set; }
        
        public DbSet<PersonPerformance> PersonPerformances { get; set; }
        
        public DbSet<PersonRole> PersonRoles { get; set; }
        
        public DbSet<Post> Posts { get; set; }
        
        public DbSet<PostComment> PostComments { get; set; }
        
        public DbSet<Poster> Posters { get; set; }
        
        public DbSet<PostOpinion> PostOpinions { get; set; }

        public DbSet<PersonComment> PersonComments { get; set; }
    }
}