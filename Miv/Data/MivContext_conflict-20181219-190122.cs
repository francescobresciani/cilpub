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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ParentChild>()
            .HasKey(u => new { u.ChildID, u.ParentID });

            modelBuilder.Entity<Material>()
                .HasMany(u => u.Children)
                .WithOne(f => f.Parent)
                .HasForeignKey(f => f.ParentID);

            modelBuilder.Entity<Material>()
                .HasMany(u => u.Parents)
                .WithOne(f => f.Child)
                .HasForeignKey(f => f.ChildID);
        }


        public DbSet<Material> Materials { get; set; }
        public DbSet<ParentChild> ParentChild { get; set; }

    }
}
