using WorkerService1;
using WorkerService1.CategoryServices;
using WorkerService1.HttpRequest;
using WorkerService1.Services; 

var builder = Host.CreateApplicationBuilder(args);

// Worker kayd�
builder.Services.AddHostedService<Worker>();

// Servis kayd� (interface - implementasyonu)
builder.Services.AddSingleton<IPokemonService, PokemonService>();
builder.Services.AddSingleton<ICategoryService, CategoryService>();

builder.Services.AddSingleton<IHttpRequest, HttpRequest>();

var host = builder.Build();
host.Run();
