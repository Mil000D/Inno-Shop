using Microsoft.EntityFrameworkCore;
using UserManagementService.Enums;
using UserManagementService.Models;

namespace UserManagementService.DAL.Context
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "Admin",
                    Email = "admin@admin.com",
                    Role = Role.Admin,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
                    IsActive = true,
                    IsVerified = true
                },
                new User
                {
                    Id = 2,
                    Name = "User",
                    Email = "user@user.com",
                    Role = Role.User,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
                    IsActive = true,
                    IsVerified = true
                }
            );
        }
    }
}
