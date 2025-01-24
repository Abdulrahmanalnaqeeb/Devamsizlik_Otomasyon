using Microsoft.EntityFrameworkCore;
using DevamsizlikTakip.Models;

namespace DevamsizlikTakip.Data
{
    public class DevamsizlikContext : DbContext
    {
        public DevamsizlikContext(DbContextOptions<DevamsizlikContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Devamsizlik> Devamsizliklar { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // İlişkileri tanımlama
            modelBuilder.Entity<Devamsizlik>()
                .HasOne(d => d.User)
                .WithMany(u => u.Devamsizliklar)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 