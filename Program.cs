using teddy_smith_api.Data;
using Microsoft.EntityFrameworkCore;
using teddy_smith_api.Interfaces;
using teddy_smith_api.Repository;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// https://www.youtube.com/watch?v=A3tdyk68KAw&list=PL82C6-O4XrHfrGOCPmKmwTO7M0avXyQKc&index=13&ab_channel=TeddySmith

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
  options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
  options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

// репозитории
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

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
