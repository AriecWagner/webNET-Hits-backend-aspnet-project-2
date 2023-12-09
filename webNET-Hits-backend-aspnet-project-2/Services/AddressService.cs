using System;
using System.Collections.Generic;
using System.Linq;
using backtestDevelop.Data;
using backtestDevelop.Models.AnotherModels;
using backtestDevelop.Models.DbModels;
using backtestDevelop.Models.EnumModels;
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
