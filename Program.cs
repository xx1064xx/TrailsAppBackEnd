using TrailsAppRappi.Interfaces;
using TrailsAppRappi.Sampledata;
using TrailsAppRappi.Data;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("*") // Replace with your specific origin(s)
                         .AllowAnyMethod()
                         .AllowAnyHeader();

                      });
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

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
