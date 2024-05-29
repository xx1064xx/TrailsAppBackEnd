namespace TrailsAppRappi.Sampledata
{
    using TrailsAppRappi.Interfaces;
    using TrailsAppRappi.Core.Entities;
    using TrailsAppRappi.Core;
    using TrailsAppRappi.Data;

    public static class TrailSamleData
    {

        public static void InitializeTrailsAppDatabase(IDbContextFactory contextFactory)
        {
            using var context = contextFactory.CreateContext();
            if (!context.Database.EnsureCreated())
            {
                return;
            }

            if (!context.Trails.Any())
            {
                InsertTestData(context);
            }
        }

        private static void InsertTestData(TrailsAppContext context)
        {

            string salt1;
            string salt2;

            string pwHash1 = HashGenerator.GenerateHash("superSecurePassword", out salt1);
            string pwHash2 = HashGenerator.GenerateHash("anotherSuperSecurePassword", out salt2);

            var user1 = new User
            {
                FirstName = "Martin",
                LastName = "Maurer",
                Email = "martin.maurer@gmail.com",
                Password = pwHash1,
                Salt = salt1
            };

            var user2 = new User
            {
                FirstName = "Merlin",
                LastName = "Löscher",
                Email = "mloescher@bluewin.ch",
                Password = pwHash2,
                Salt = salt2
            };

            context.Users.Add(user1);
            context.Users.Add(user2);
            context.SaveChanges();


            var trail1 = new Trail
            {
                Location = "Zürich",
                Name = "MitDeBres",
                DateAndTime = new DateTime(2024, 8, 13),
                User = user1
            };

            var trail2 = new Trail
            {
                Location = "Geneva",
                Name = "LakeView",
                DateAndTime = new DateTime(2024, 7, 21),
                User = user1
            };

            var trail3 = new Trail
            {
                Location = "Bern",
                Name = "HistoricRoute",
                DateAndTime = new DateTime(2024, 9, 5),
                User = user1
            };

            var trail4 = new Trail
            {
                Location = "Basel",
                Name = "RhineRiverWalk",
                DateAndTime = new DateTime(2024, 6, 18),
                User = user1
            };

            var trail5 = new Trail
            {
                Location = "Lausanne",
                Name = "MountainTrail",
                DateAndTime = new DateTime(2024, 10, 3),
                User = user2
            };

            var trail6 = new Trail
            {
                Location = "Lucerne",
                Name = "CityCenterPath",
                DateAndTime = new DateTime(2024, 8, 27),
                User = user2
            };


            context.Trails.Add(trail1);
            context.Trails.Add(trail2);
            context.Trails.Add(trail3);
            context.Trails.Add(trail4);
            context.Trails.Add(trail5);
            context.Trails.Add(trail6);

            context.SaveChanges();

        }



    }
}
