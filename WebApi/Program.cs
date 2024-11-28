using System.Text;
using Application;
using Application.Identity;
using Application.Interfaces;
using Application.Options;
using Application.Services;
using Domain.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Persistence.Repositories;
using WebApi.ExceptionHandlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IIncomeRepository, IncomeRepository>();
builder.Services.AddTransient<IExpenseRepository, ExpenseRepository>();
builder.Services.AddTransient<IGoalRepository, GoalRepository>();
builder.Services.AddTransient<IBudgetRepository, BudgetRepository>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddTransient<IGoalService, GoalService>();
builder.Services.AddTransient<IBudgetService, BudgetService>();
builder.Services.AddTransient<IExpenseService, ExpenseService>();
builder.Services.AddTransient<IIncomeService, IncomeService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IJwtService, JwtService>();

builder.Services.AddAutoMapper(typeof(MapperProfile));

builder.Services.AddDbContext<FinanceContext>(o =>
{
    var connectionString = builder.Configuration.GetConnectionString("Finance");
    o.UseNpgsql(connectionString);
});


var jwtOptions = builder.Configuration.GetSection("JwtOptions").Get<JwtOptions>();
builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }
    )
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions!.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
        };
    });


builder.Services.Configure<GoogleOptions>(builder.Configuration.GetSection("GoogleOptions"));
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

builder.Services.AddExceptionHandler<BadRequestExceptionHandler>();
builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
builder.Services.AddExceptionHandler<ForbiddenExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();


builder.Services.AddIdentityCore<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<FinanceContext>()
    .AddSignInManager<SignInManager<User>>();

builder.Services.AddSingleton(TimeProvider.System);

var corsPolicyName = "AllowForFrontend";
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: corsPolicyName,
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowCredentials()
                .AllowAnyMethod();
        });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(corsPolicyName);

app.UseExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();