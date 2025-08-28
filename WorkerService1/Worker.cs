using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using WorkerService1.CategoryServices;
using WorkerService1.Services;

namespace WorkerService1
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IPokemonService _pokemonService;
        private readonly ICategoryService _categoryService;

        public Worker(ILogger<Worker> logger, IPokemonService pokemonService,ICategoryService categoryService)
        {
            _logger = logger;
            _pokemonService = pokemonService;
            _categoryService =categoryService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _pokemonService.LoginAsync("stitich@gmail.com", "123");

            while (!stoppingToken.IsCancellationRequested)
            {
                await _pokemonService.GetAndSavePokemonAsync();
                await _categoryService.GetAndSaveCategoryAsync();

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
