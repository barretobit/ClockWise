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
    options.AddPolicy("AllowLocalhostAndClockWise",
        builder =>
        {
            builder.SetIsOriginAllowed(origin =>
            {
                // Allow localhost origins
                if (origin.StartsWith("http://localhost:") || origin.StartsWith("https://localhost:"))
                {
                    return true;
                }

                // Allow your deployed origin
                if (origin == "https://clockwiseweb.runasp.net")
                {
                    return true;
                }

                return false; // Deny other origins
            })
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
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

app.UseCors("AllowLocalhostAndClockWise");

app.MapControllers();

app.Run();