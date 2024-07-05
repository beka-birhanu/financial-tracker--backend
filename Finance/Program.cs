using Finance.Data;
using Finance.Models;
using Finance.Services.Expenses;
using Finance.Services.FilterSort;
using Finance.Services.Pagination;
using Finance.Services.Auth;
using Finance.Services.JWT;
using Microsoft.EntityFrameworkCore;
using Finance.Services.Hashing;

var builder = WebApplication.CreateBuilder(args);
{
  var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
  builder.Services.AddDbContext<ExpenseContext>(options => options.UseSqlite(connectionString));
  builder.Services.AddDbContext<UserContext>(options => options.UseSqlite(connectionString));

  builder.Services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);

  builder.Services.AddScoped<IExpenseService, ExpenseService>();
  builder.Services.AddScoped<IJwtService, JwtService>();
  builder.Services.AddScoped<IAuthService, AuthService>();
  builder.Services.AddScoped<IHashService, HashService>();

  builder.Services.AddScoped<IFilterSortStrategy<Expense>, FilterSortStrategy<Expense>>();
  builder.Services.AddScoped<IPaginationStrategy<Expense>, PaginationStrategy<Expense>>();
}

var app = builder.Build();
{
  app.UseExceptionHandler("/error");
  app.UseHttpsRedirection();
  app.UseStatusCodePages();
  app.MapControllers();
  app.Run();
}

