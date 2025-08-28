
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WorkerService1.Dto;
using WorkerService1.HttpRequest;
using WorkerService1.Models;


namespace WorkerService1.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly IHttpRequest _httpRequest;
        private readonly HashSet<int> _writtenPokemonIds = new();
        public PokemonService(IHttpRequest httpRequest)
        {
            _httpRequest = httpRequest;
        }

        public async Task LoginAsync(string username, string password)
        {
            var loginData = new LoginRequest
            {
                Email = username,
                Password = password
            };

            var tokenResponse = await _httpRequest.PostAsync<TokenResponse>("api/Auth/login", loginData);

            if (tokenResponse == null)
            {
                throw new Exception("Login başarısız: API'den geçerli cevap alınamadı.");
            }

            string token = tokenResponse.Token
                           ?? tokenResponse.AccessTokenSnake
                           ?? tokenResponse.AccessTokenCamel
                           ?? tokenResponse.Jwt
                           ?? tokenResponse.BearerToken
                           ?? tokenResponse.data?.token;

            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("Login başarısız: Token parse edilemedi.");
            }

            SessionManager.Token = token;

            // BURASI YENİ: Token'ı console'a yazdır
            Console.WriteLine($"Giriş başarılı. Alınan Token: {token}");
        }


        public async Task GetAndSavePokemonAsync()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pokemon.txt");
            Console.WriteLine($"Dosya yolu: {filePath}");

            try
            {
                var pokemons = await _httpRequest.GetAllAsync<List<PokemonDto>>("/api/Pokemon");
                Console.WriteLine($"API'den gelen veri sayısı: {pokemons?.Count ?? 0}");

                if (pokemons == null || pokemons.Count == 0)
                {
                    await File.AppendAllTextAsync(filePath, $"{DateTime.Now}: Hata - Veri boş{Environment.NewLine}");
                    return;
                }

                // Pokemonları tek bir satırda topluca yazmak için StringBuilder kullanıyoruz.
                var newPokemonEntries = new StringBuilder();

                // Pokemonları tek bir satırda yazdır
                foreach (var pokemon in pokemons)
                {
                    if (_writtenPokemonIds.Contains(pokemon.Id))
                    {
                        continue; // Daha önce yazılmış, atla
                    }

                    string entry = $"{pokemon.Id} {pokemon.Name} {pokemon.BirthDate}, ";
                    newPokemonEntries.Append(entry);

                    _writtenPokemonIds.Add(pokemon.Id); // Belleğe ekle
                }

                if (newPokemonEntries.Length > 0)
                {
                    // Bu satırı birleştirip tek bir satır olarak yazdırıyoruz
                    string pokemonData = $"{DateTime.Now}: {newPokemonEntries.ToString().TrimEnd(',', ' ')}{Environment.NewLine}";
                    await File.AppendAllTextAsync(filePath, pokemonData);
                    Console.WriteLine($"Yeni pokemonlar başarıyla tek bir satırda yazıldı.");
                }
                else
                {
                    Console.WriteLine("Yeni pokemon bulunamadı.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Dosyaya yazma sırasında hata oluştu: {ex.Message}");
            }
        }



    }
}
