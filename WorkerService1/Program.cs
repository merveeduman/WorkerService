using WorkerService1;
using WorkerService1.CategoryServices;
using WorkerService1.HttpRequest;
using WorkerService1.Services; 

var builder = Host.CreateApplicationBuilder(args);

// Worker kaydý
builder.Services.AddHostedService<Worker>();

// Servis kaydý (interface - implementasyonu)
builder.Services.AddSingleton<IPokemonService, PokemonService>();
builder.Services.AddSingleton<ICategoryService, CategoryService>();

builder.Services.AddSingleton<IHttpRequest, HttpRequest>();

var host = builder.Build();
host.Run();
