using System;
using System.Collections.Generic;
using System.Linq;
using webNET_Hits_backend_aspnet_project_2.Data;
using webNET_Hits_backend_aspnet_project_2.Models.DbModels;
using webNET_Hits_backend_aspnet_project_2.Models.EnumModels;
using Microsoft.EntityFrameworkCore;

namespace backtestDevelop.Services
{
    public class AddressService
    {
        private readonly AddressDbContext _dbContext;

        public AddressService(AddressDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
