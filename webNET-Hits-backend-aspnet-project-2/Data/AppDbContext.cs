﻿using webNET_Hits_backend_aspnet_project_2.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace webNET_Hits_backend_aspnet_project_2.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserData> Users { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }

}
