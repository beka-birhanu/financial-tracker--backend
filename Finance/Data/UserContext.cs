using Finance.Data.EntityConfigurations;
using Finance.Models;
using Microsoft.EntityFrameworkCore;

namespace Finance.Data;

public class UserContext : DbContext
{
  public UserContext(DbContextOptions<UserContext> options) : base(options)
  {
  }

  public DbSet<User> Users { get; set; } = null!;

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfiguration(new UserConfiguration());
  }
}
