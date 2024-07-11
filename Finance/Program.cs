using Finance.Data;
using Finance.Models;
using Finance.Services.Expenses;
using Finance.Services.FilterSort;
using Finance.Services.Pagination;
using Finance.Services.Auth;
using Finance.Services.JWT;
using Finance.Services.Hashing;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("DefaultConnection is not configured.");

var jwtSecret = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key is not configured.");


// Services registration
builder.Services.AddDbContext<ExpenseContext>(options => options.UseSqlite(connectionString));
builder.Services.AddDbContext<UserContext>(options => options.UseSqlite(connectionString));

builder.Services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);

builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddSingleton<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<IHashService, HashService>();

builder.Services.AddScoped<IFilterSortStrategy<Expense>, FilterSortStrategy<Expense>>();
builder.Services.AddScoped<IPaginationStrategy<Expense>, PaginationStrategy<Expense>>();

// Authentication configuration
builder.Services.AddAuthentication(options =>
{
  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
  options.TokenValidationParameters = new TokenValidationParameters
  {
    ValidateIssuer = false,
    ValidateAudience = false,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
  };
  options.Events = new JwtBearerEvents
  {
    OnMessageReceived = context =>
    {
      context.Token = context.Request.Cookies["access-token"];
      return Task.CompletedTask;
    }
  };
});

var app = builder.Build();

// Middleware configuration
app.UseExceptionHandler("/error");
app.UseHttpsRedirection();
app.UseStatusCodePages();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

