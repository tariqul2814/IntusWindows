using IntusWindowsInterview.Model.DBModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntusWindowsInterview.Model.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        { }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Window> Windows { get; set; }
        public DbSet<SubElement> SubElements { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(x =>
            {
                x.HasKey("Id");
                x.HasMany(c => c.Windows)
                .WithOne(g => g.Order);
            });

            modelBuilder.Entity<Window>(x =>
            {
                x.HasKey("Id");
                x.HasMany(c => c.SubElements)
                .WithOne(g => g.Window);
            });

            modelBuilder.Entity<SubElement>(x => x.HasKey("Id"));

            base.OnModelCreating(modelBuilder);
        }
    }
}
