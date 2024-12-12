namespace DogInfo.Application.DTOs
{
    public class BreedDto
    {
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public int LifeSpanMin { get; set; }
        public int LifeSpanMax { get; set; }
        public int MaleWeightMin { get; set; }
        public int MaleWeightMax { get; set; }
        public int FemaleWeightMin { get; set; }
        public int FemaleWeightMax { get; set; }
        public bool Hypoallergenic { get; set; }
    }
}
