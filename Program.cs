using teddy_smith_api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// https://www.youtube.com/watch?v=SIQhe-yt3mA&list=PL82C6-O4XrHfrGOCPmKmwTO7M0avXyQKc&index=3&ab_channel=TeddySmith

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
  options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
  app.UseSwaggerUI(options =>
  {
    options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1");
  });
}
else
{
  app.UseHttpsRedirection();
}

app.MapControllers();

app.Run();
