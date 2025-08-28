using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerService1.Dto;
using WorkerService1.HttpRequest;

namespace WorkerService1.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly IHttpRequest _httpRequest;
        private readonly HashSet<int> _writtenCategoryIds = new();
        public CategoryService(IHttpRequest httpRequest)
        {
            _httpRequest = httpRequest;
        }
        public async Task GetAndSaveCategoryAsync()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "category.txt");
            Console.WriteLine($"Dosya yolu: {filePath}");

            try
            {
                // Kategori verilerini al
                var categories = await _httpRequest.GetAllAsync<List<CategoryDto>>("/api/Category");
                Console.WriteLine($"API'den gelen Category veri sayısı: {categories?.Count ?? 0}");

                if (categories == null || categories.Count == 0)
                {
                    await File.AppendAllTextAsync(filePath, $"{DateTime.Now}: Hata - Veri boş{Environment.NewLine}");
                    return;
                }

                // Kategorileri tek bir satırda yazdırmak için StringBuilder kullanıyoruz
                var newCategoryEntries = new StringBuilder();

                // Kategorileri işle
                foreach (var category in categories)
                {
                    if (_writtenCategoryIds.Contains(category.Id))
                    {
                        continue; // Daha önce yazılmış, atla
                    }
                    string categoryEntry = $"{category.Id} {category.Name}, ";
                    newCategoryEntries.Append(categoryEntry);
                    _writtenCategoryIds.Add(category.Id); // Belleğe ekle
                }

                // Kategori verilerini birleştirip tek bir satırda yazdırıyoruz
                if (newCategoryEntries.Length > 0)
                {
                    string categoryData = $"{DateTime.Now}: Kategoriler -> {newCategoryEntries.ToString().TrimEnd(',', ' ')}{Environment.NewLine}";
                    await File.AppendAllTextAsync(filePath, categoryData);
                    Console.WriteLine($"Yeni kategoriler başarıyla tek bir satırda yazıldı.");
                }
                else
                {
                    Console.WriteLine("Yeni kategoriler bulunamadı.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Dosyaya yazma sırasında hata oluştu: {ex.Message}");
            }
        }


    }
}
