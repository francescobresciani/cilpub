using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CadViewer.Models
{
    public class CadViewerContext : DbContext
    {
        public CadViewerContext (DbContextOptions<CadViewerContext> options)
            : base(options)
        {
        }

        public DbSet<CadViewer.Models.Material> Material { get; set; }
    }
}
