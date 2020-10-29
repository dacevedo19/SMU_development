using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace SMU.Models
{
    public class AppDbContext : IdentityDbContext
    {

        public DbSet<AppUser> AppUsers { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=localhost; Port=5432; Database=db_SMU; Username =SMU_user; Password=password123");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<AppUser>().HasIndex(u => u.Document).IsUnique();     
            
            
        }

        
        //DbSets
        public DbSet<SMU.Models.Request> Requests { get; set; }
        public DbSet<SMU.Models.UserClaim> Claims { get; set; }
                
    }
}
