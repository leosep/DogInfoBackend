using DogInfo.Application.DTOs;
using DogInfo.Application.Interfaces;
using System.Net.Http;
using System.Text.Json;

namespace DogInfo.Infrastructure.Services
{
    public class DogApiService : IDogApiService
    {
        private readonly HttpClient _httpClient;

        public DogApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<BreedDto>> GetBreedsAsync()
        {
            var response = await _httpClient.GetStringAsync("https://dogapi.dog/api/v2/breeds");
            var json = JsonDocument.Parse(response);
            var breeds = new List<BreedDto>();

            foreach (var element in json.RootElement.GetProperty("data").EnumerateArray())
            {
                var attributes = element.GetProperty("attributes");
                breeds.Add(new BreedDto
                {
                    Name = attributes.GetProperty("name").GetString()!,
                    Description = attributes.GetProperty("description").GetString()!,
                    LifeSpanMin = attributes.GetProperty("life").GetProperty("min").GetInt32(),
                    LifeSpanMax = attributes.GetProperty("life").GetProperty("max").GetInt32(),
                    MaleWeightMin = attributes.GetProperty("male_weight").GetProperty("min").GetInt32(),
                    MaleWeightMax = attributes.GetProperty("male_weight").GetProperty("max").GetInt32(),
                    FemaleWeightMin = attributes.GetProperty("female_weight").GetProperty("min").GetInt32(),
                    FemaleWeightMax = attributes.GetProperty("female_weight").GetProperty("max").GetInt32(),
                    Hypoallergenic = attributes.GetProperty("hypoallergenic").GetBoolean(),
                });
            }

            return breeds;
        }
    }
}
