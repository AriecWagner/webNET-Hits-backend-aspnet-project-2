using webNET_Hits_backend_aspnet_project_2.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using System;

namespace webNET_Hits_backend_aspnet_project_2.Data
{
    public class AddressDbContext : DbContext
    {
        public DbSet<AsAdmHierarchy> as_adm_hierarchy { get; set; }
        public DbSet<AsAddrObj> as_addr_obj { get; set; }
        public DbSet<AsHouses> as_houses { get; set; }

        public AddressDbContext(DbContextOptions<AddressDbContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }
    }
}
