using DogInfo.Application.DTOs;

namespace DogInfo.Application.Interfaces
{
    public interface IDogApiService
    {
        Task<IEnumerable<BreedDto>> GetBreedsAsync();
    }
}
