﻿namespace TrailsAppRappi.Core.Entities
{
    public class User
    {

        public Guid UserId { get; set; } = new Guid();
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Salt { get; set; }


        public ICollection<Trail> Trails { get; set; }
    }
}
