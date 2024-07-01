using Finance.Data;
using Finance.Services.Expenses;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
{
  var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
  builder.Services.AddDbContext<ExpenseContext>(options => options.UseSqlite(connectionString));
  builder.Services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);
  builder.Services.AddScoped<IExpenseService, ExpenseService>();
}

var app = builder.Build();
{
  app.UseExceptionHandler("/error");
  app.UseHttpsRedirection();
  app.UseStatusCodePages();
  app.MapControllers();
  app.Run();
}
