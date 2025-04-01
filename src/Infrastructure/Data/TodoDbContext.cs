using Domain.Entities;
using Domain.Entities.ApplicationIdentity;
using Infrastructure.Data.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class TodoDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new TodoConfiguration());
            base.OnModelCreating(builder);
           
            
            builder.Entity<ApplicationUser>().ToTable("AppUsers");
            builder.Entity<ApplicationRole>().ToTable("AppRoles");
        }

        public DbSet<Todo> Todos { get; set; }

    }
}
