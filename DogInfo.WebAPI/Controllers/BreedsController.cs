using DogInfo.Domain.Entities;
using DogInfo.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DogInfo.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BreedsController : ControllerBase
    {
        private readonly IRepository<Breed> _breedRepository;

        public BreedsController(IRepository<Breed> breedRepository)
        {
            _breedRepository = breedRepository;
        }

        // GET: api/breeds?pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetBreeds([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("Page number and page size must be greater than zero.");
            }

            var breeds = await _breedRepository.GetAllAsync();
            var totalItems = breeds.Count(); // Total number of items
            var pagedBreeds = breeds
                .Skip((pageNumber - 1) * pageSize) // Skip items from previous pages
                .Take(pageSize) // Take items for the current page
                .ToList();

            var response = new
            {
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Items = pagedBreeds
            };

            return Ok(response);
        }

        // GET: api/breeds/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBreedById(int id)
        {
            var breed = await _breedRepository.GetByIdAsync(id);
            if (breed == null)
            {
                return NotFound(); 
            }

            return Ok(breed);
        }

        // POST: api/breeds
        [HttpPost]
        public async Task<IActionResult> AddBreed([FromBody] Breed breed)
        {
            if (breed == null)
            {
                return BadRequest("Breed is null");
            }

            await _breedRepository.AddAsync(breed);

            return CreatedAtAction(nameof(GetBreedById), new { id = breed.Id }, breed);
        }

        // PUT: api/breeds/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBreed(int id, [FromBody] Breed breed)
        {
            if (breed == null)
            {
                return BadRequest("Breed is null");
            }

            // Check if breed exists
            var existingBreed = await _breedRepository.GetByIdAsync(id);
            if (existingBreed == null)
            {
                return NotFound();  // Return 404 if breed not found
            }

            // Update the breed
            existingBreed.Name = breed.Name;
            existingBreed.Description = breed.Description;
            existingBreed.LifeSpanMin = breed.LifeSpanMin;
            existingBreed.LifeSpanMax = breed.LifeSpanMax;
            existingBreed.MaleWeightMin = breed.MaleWeightMin;
            existingBreed.MaleWeightMax = breed.MaleWeightMax;
            existingBreed.FemaleWeightMin = breed.FemaleWeightMin;
            existingBreed.FemaleWeightMax = breed.FemaleWeightMax;
            existingBreed.Hypoallergenic = breed.Hypoallergenic;

            await _breedRepository.UpdateAsync(existingBreed);

            return NoContent();
        }

        // DELETE: api/breeds/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBreed(int id)
        {
            var breed = await _breedRepository.GetByIdAsync(id);
            if (breed == null)
            {
                return NotFound();
            }

            await _breedRepository.DeleteAsync(breed);

            return NoContent();
        }
    }
}
