using Microsoft.EntityFrameworkCore;
using Blogic_task.Models;

namespace Blogic_task.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Klient> Klienti { get; set; }
        public DbSet<Poradce> Poradci { get; set; }
        public DbSet<Smlouva> Smlouvy { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Smlouva>()
                .HasOne(s => s.Spravce)
                .WithMany(p => p.SpravovaneSmlouvy)
                .HasForeignKey(s => s.SpravceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Smlouva>()
                .HasMany(s => s.DalsiPoradci)
                .WithMany(p => p.DalsiSmlouvy)
                .UsingEntity(j => j.ToTable("SmlouvaDalsiPoradce"));
        }
    }
}