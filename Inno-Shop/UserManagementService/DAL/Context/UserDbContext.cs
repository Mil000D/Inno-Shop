﻿using Microsoft.EntityFrameworkCore;
using UserManagementService.Models;

namespace UserManagementService.DAL.Context
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
    }
}
