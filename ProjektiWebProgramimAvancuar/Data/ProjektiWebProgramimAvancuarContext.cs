using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjektiWebProgramimAvancuar.Models;

namespace ProjektiWebProgramimAvancuar.Data
{
    public class ProjektiWebProgramimAvancuarContext : DbContext
    {
        public ProjektiWebProgramimAvancuarContext (DbContextOptions<ProjektiWebProgramimAvancuarContext> options)
            : base(options)
        {
        }

        public DbSet<ProjektiWebProgramimAvancuar.Models.Comment> Comment { get; set; } = default!;

        public DbSet<ProjektiWebProgramimAvancuar.Models.Post>? Post { get; set; }

        public DbSet<ProjektiWebProgramimAvancuar.Models.User>? User { get; set; }
    }
}
