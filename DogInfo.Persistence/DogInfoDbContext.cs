using DogInfo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DogInfo.Persistence
{
    public class DogInfoDbContext : DbContext
    {
        public DogInfoDbContext(DbContextOptions<DogInfoDbContext> options)
            : base(options) 
        { }

        public DbSet<Breed> Breeds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
