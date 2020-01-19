using DailyRandom.Data.Tables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyRandom.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Draw> Draws { get; set; }
        public DbSet<CodliOption> CodliOptions { get; set; }
        public DbSet<ApplicationClient> ApplicationClients { get; set; }
    }
}
