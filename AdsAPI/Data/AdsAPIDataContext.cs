using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AdsAPI.Models;

    public class AdsAPIDataContext : DbContext
    {
        public AdsAPIDataContext (DbContextOptions<AdsAPIDataContext> options)
            : base(options)
        {
        }

        public DbSet<AdsAPI.Models.Ad> Ad { get; set; } = default!;
    }
