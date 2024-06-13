using TrailsAppRappi.Interfaces;
using TrailsAppRappi.Sampledata;
using TrailsAppRappi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TrailsAppRappi.Controllers;
using System.Text;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173")
                         .AllowAnyMethod()
                         .AllowAnyHeader();

                      });
});

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {

            ValidIssuer = JwtConfiguration.ValidIssuer,
            ValidAudience = JwtConfiguration.ValidAudience,
            IssuerSigningKey = new SymmetricSecurityKey(
               Encoding.UTF8.GetBytes(JwtConfiguration.IssuerSigningKey)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IDbContextFactory, DbContextFactory>();

var app = builder.Build();

// InizializeDb
using var scope = app.Services.CreateScope();
{
    var contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory>();
    TrailSamleData.InitializeTrailsAppDatabase(contextFactory);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

    await next();
});

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
