namespace TrailsAppRappi.Core.Entities
{
    public class Trail
    {
        public Guid TrailId { get; set; } = new Guid();
        public string? Location { get; set; }
        public string? Name { get; set; }
        public DateTime? DateAndTime { get; set; }
        public Guid UserId { get; set; }


        public User User { get; set; }
    }
}
