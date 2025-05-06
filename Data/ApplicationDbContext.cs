using ChartJsVeriGorsellestirme.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ChartJsVeriGorsellestirme.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                 : base(options)
        {
        }

        public DbSet<Cekilis> SuperLotoSonuclari { get; set; }
    }
}
