using DogInfo.Application.Interfaces;
using DogInfo.Domain.Entities;
using Hangfire;
using Microsoft.Extensions.Logging;

namespace DogInfo.Jobs
{
    public class BreedDataUpsertJob
    {
        private readonly ILogger<BreedDataUpsertJob> _logger;
        private readonly IDogApiService _dogApiService;
        private readonly IRepository<Breed> _breedRepository;

        public BreedDataUpsertJob(
            ILogger<BreedDataUpsertJob> logger,
            IDogApiService dogApiService,
            IRepository<Breed> breedRepository)
        {
            _logger = logger;
            _dogApiService = dogApiService;
            _breedRepository = breedRepository;
        }

        [JobDisplayName("Breed Data Upsert Job")]
        public async Task ExecuteAsync()
        {
            _logger.LogInformation("Starting Breed Data Upsert Job.");

            try
            {
                var breeds = await _dogApiService.GetBreedsAsync();

                foreach (var breed in breeds)
                {
                    // Check if the breed exists
                    var existingBreed = (await _breedRepository.FindAsync(b => b.Name == breed.Name))
                        .FirstOrDefault();

                    if (existingBreed == null)
                    {
                        // Add new breed
                        await _breedRepository.AddAsync(new Breed
                        {
                            Name = breed.Name,
                            Description = breed.Description,
                            LifeSpanMin = breed.LifeSpanMin,
                            LifeSpanMax = breed.LifeSpanMax,
                            MaleWeightMin = breed.MaleWeightMin,
                            MaleWeightMax = breed.MaleWeightMax,
                            FemaleWeightMin = breed.FemaleWeightMin,
                            FemaleWeightMax = breed.FemaleWeightMax,
                            Hypoallergenic = breed.Hypoallergenic
                        });
                    }
                    else
                    {
                        // Update existing breed
                        existingBreed.Description = breed.Description;
                        existingBreed.LifeSpanMin = breed.LifeSpanMin;
                        existingBreed.LifeSpanMax = breed.LifeSpanMax;
                        existingBreed.MaleWeightMin = breed.MaleWeightMin;
                        existingBreed.MaleWeightMax = breed.MaleWeightMax;
                        existingBreed.FemaleWeightMin = breed.FemaleWeightMin;
                        existingBreed.FemaleWeightMax = breed.FemaleWeightMax;
                        existingBreed.Hypoallergenic = breed.Hypoallergenic;

                        await _breedRepository.UpdateAsync(existingBreed);
                    }
                }

                _logger.LogInformation($"Successfully processed {breeds.Count()} breeds.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Breed Data Upsert Job: {ex.Message}");
            }
        }
    }
}
