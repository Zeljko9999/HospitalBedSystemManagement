namespace BedTrack.Domain.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Alarm { get; set; }
        public ICollection<UserEvent>? AlarmEvents { get; set; }
    }
}
