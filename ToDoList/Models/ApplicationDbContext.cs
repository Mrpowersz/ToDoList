using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ToDoListing> ToDos { get; set; } = null!;
        public DbSet<Status> Statuses { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			modelBuilder.Entity<Status>().HasData(
		new Status { StatusId = "open", Name = "Open" },
		new Status { StatusId = "closed", Name = "Completed" }
	);
		}
    }
}