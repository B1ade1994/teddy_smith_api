using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using teddy_smith_api.Models;

namespace teddy_smith_api.Data
{
  public class ApplicationDbContext : IdentityDbContext<User>
  {
    public ApplicationDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {

    }

    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Portfolio> Portfolios { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder.Entity<Portfolio>(x => x.HasKey(p => new { p.UserId, p.StockId }));
      builder.Entity<Portfolio>().HasOne(p => p.User).WithMany(p => p.Portfolios).HasForeignKey(p => p.UserId);
      builder.Entity<Portfolio>().HasOne(p => p.Stock).WithMany(p => p.Portfolios).HasForeignKey(p => p.StockId);

      List<IdentityRole> roles = new List<IdentityRole>
      {
        new IdentityRole
        {
          Id = "Admin",
          Name = "Admin",
          NormalizedName = "ADMIN"
        },
        new IdentityRole
        {
          Id = "User",
          Name = "User",
          NormalizedName = "USER"
        },
      };
      builder.Entity<IdentityRole>().HasData(roles);
    }
  }
}