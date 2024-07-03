namespace BedTrack.Configuration
{
    public class ValidationConfiguration
    {
        public int? NameMaxCharacters { get; set; }
        public int? DescriptionMaxCharacters { get; set; }
        public int? EmailMaxCharacters { get; set; }
        public string? EmailRegex { get; set; }
    }
}
