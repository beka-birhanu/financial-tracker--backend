using Finance.Services.Expenses;

var builder = WebApplication.CreateBuilder(args);
{
  builder.Services.AddControllers();
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
