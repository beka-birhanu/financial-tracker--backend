using Finance.Models;
using Microsoft.EntityFrameworkCore;

namespace Finance.Data;

public class ExpenseContext : DbContext
{
  public ExpenseContext(DbContextOptions<ExpenseContext> options) : base(options)
  {
  }

  public DbSet<Expense> Expenses { get; set; } = null!;
}
