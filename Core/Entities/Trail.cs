namespace TrailsAppRappi.Core.Entities
{
    public class Trail
    {
        public Guid TrailId { get; set; } = new Guid();
        public string? Location { get; set; }
        public string? Name { get; set; }
        public DateTime? DateAndTime { get; set; }
    }
}
