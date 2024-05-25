using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NoteLite.Models
{
    public class NoteDBContext :IdentityDbContext
    {
        public NoteDBContext()
        {
            
        }
        public NoteDBContext(DbContextOptions<NoteDBContext> options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserLogin<string>>().HasNoKey();
            modelBuilder.Entity<Tag>()
            .HasOne(t => t.Note)
            .WithMany(n => n.Tags)
            .HasForeignKey(t => t.NoteId)
            .OnDelete(DeleteBehavior.Cascade);
            base.OnModelCreating(modelBuilder);
        }

    }
}
