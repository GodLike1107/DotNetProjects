using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NeighborhoodServices.API.Data;
using NeighborhoodServices.API.Models;
using NeighborhoodServices.API.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// ✅ Clear default claim mapping
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

// ✅ Use Newtonsoft.Json for cleaner serialization (no $id/$values)
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
    });

// ✅ Custom services
builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();
builder.Services.AddScoped<IJwtService, JwtService>();

// ✅ Swagger config
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "NeighborhoodServices API",
        Version = "v1",
        Description = "API for managing service bookings"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Name = "Bearer",
                In = ParameterLocation.Header,
                Scheme = "Bearer"
            },
            new List<string>()
        }
    });
});

// ✅ CORS for Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy => policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

// ✅ EF Core
builder.Services.AddDbContext<NeighborhoodDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ JWT Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = builder.Configuration["Jwt:Key"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!)),

            NameClaimType = ClaimTypes.NameIdentifier,
            RoleClaimType = ClaimTypes.Role
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// ✅ Middleware
app.UseCors("AllowAngularApp");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "NeighborhoodServices API v1");
        c.RoutePrefix = string.Empty; // Loads Swagger at root
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// ✅ Optional data seeding
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<NeighborhoodDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    var provider = context.Users.FirstOrDefault(u => u.Role == "Provider");
    if (provider != null)
    {
        var jane = context.Users.FirstOrDefault(u => u.Email == "jane@example.com");
        if (jane != null && !context.ServiceBookings.Any(b => b.CustomerId == jane.Id))
        {
            var service = context.Services.FirstOrDefault();
            if (service != null)
            {
                context.ServiceBookings.Add(new ServiceBooking
                {
                    CustomerId = jane.Id,
                    ServiceId = service.Id,
                    ScheduledTime = DateTime.Now.AddDays(1),
                    Status = "Confirmed"
                });
                context.SaveChanges();
                logger.LogInformation("✅ Seeded a booking for Jane.");
            }
        }
    }
}

app.Run();
