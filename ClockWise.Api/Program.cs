using System.Text;
using ClockWise.Api.Data;
using ClockWise.Api.Repositories;
using ClockWise.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Dependency Injection
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeTypeRepository, EmployeeTypeRepository>();
builder.Services.AddScoped<ITickLogsInterface, TickLogRepository>();

// Adding VueJs Compatibility
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp",
        builder => builder.WithOrigins("http://localhost:8080")
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

// Logging
builder.Logging.AddConsole();

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddDbContext<ClockWiseDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ClockWiseConnection")));

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"]
        };
    });
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ClockWiseDbContext>();
    DbInitializer.Initialize(context);
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowVueApp");

app.MapControllers();

app.Run();