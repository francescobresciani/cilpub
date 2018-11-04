using System;
using Miv.Models;
using Microsoft.EntityFrameworkCore;

namespace Miv.Data
{
    public class MivContext: DbContext
    {
        public MivContext(DbContextOptions<MivContext> options) : base(options)
        {
        }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Attaching> Attachings { get; set; }
        public DbSet<Content> Contents { get; set; }
    }
}
