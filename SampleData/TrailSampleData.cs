namespace TrailsAppRappi.Sampledata
{
    using TrailsAppRappi.Interfaces;
    using TrailsAppRappi.Core.Entities;
    using TrailsAppRappi.Core;

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
            var trail1 = new Trail
            {
                Location = "Zürich",
                Name = "MitDeBres",
                DateAndTime = new DateTime(2024, 8, 13)
            };

            var trail2 = new Trail
            {
                Location = "Geneva",
                Name = "LakeView",
                DateAndTime = new DateTime(2024, 7, 21)
            };

            var trail3 = new Trail
            {
                Location = "Bern",
                Name = "HistoricRoute",
                DateAndTime = new DateTime(2024, 9, 5)
            };

            var trail4 = new Trail
            {
                Location = "Basel",
                Name = "RhineRiverWalk",
                DateAndTime = new DateTime(2024, 6, 18)
            };

            var trail5 = new Trail
            {
                Location = "Lausanne",
                Name = "MountainTrail",
                DateAndTime = new DateTime(2024, 10, 3)
            };

            var trail6 = new Trail
            {
                Location = "Lucerne",
                Name = "CityCenterPath",
                DateAndTime = new DateTime(2024, 8, 27)
            };

            var trail7 = new Trail
            {
                Location = "Winterthur",
                Name = "NatureReserveHike",
                DateAndTime = new DateTime(2024, 11, 11)
            };

            var trail8 = new Trail
            {
                Location = "St. Gallen",
                Name = "CathedralRoute",
                DateAndTime = new DateTime(2024, 5, 14)
            };

            var trail9 = new Trail
            {
                Location = "Lugano",
                Name = "LakefrontWalk",
                DateAndTime = new DateTime(2024, 12, 2)
            };

            var trail10 = new Trail
            {
                Location = "Thun",
                Name = "CastleViewTrail",
                DateAndTime = new DateTime(2024, 4, 22)
            };


            context.Trails.Add(trail1);
            context.Trails.Add(trail2);
            context.Trails.Add(trail3);
            context.Trails.Add(trail4);
            context.Trails.Add(trail5);
            context.Trails.Add(trail6);
            context.Trails.Add(trail7);
            context.Trails.Add(trail8);
            context.Trails.Add(trail9);
            context.Trails.Add(trail10);

            context.SaveChanges();

        }



    }
}
