using BusinessLogic;
using BusinessLogic.Strategies;
using DataAccess;
using Web.Common;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// business logic
builder.Services.AddTransient<ShareService>();
builder.Services.AddTransient<FifoCostStrategy>();
builder.Services.AddSingleton<CostStrategyFactory>();

// data access logic
builder.Services.AddScoped<IShareRepository, ShareRepository>();

WebApplication app = builder.Build();

app.UseExceptionHandler(_ => { });

app.UseSwagger();
app.UseSwaggerUI();

app.UseDefaultFiles(); 
app.UseStaticFiles();

app.UseRouting();
app.MapControllers();

app.Run();

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public partial class Program; // NOTE: Used to be able to access the tests to create the application.
