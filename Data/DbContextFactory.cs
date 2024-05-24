namespace TrailsAppRappi.Data
{
    using Microsoft.EntityFrameworkCore;
    using System.Reflection;
    using TrailsAppRappi.Interfaces;
    using TrailsAppRappi.Core;
    using Microsoft.EntityFrameworkCore.Internal;
    using TrailsAppRappi.Core;

    public class DbContextFactory : IDbContextFactory
    {
        private static readonly Assembly ConfigurationsAssembly = typeof(DbContextFactory).Assembly;
        private readonly DbContextOptionsBuilder<TrailsAppContext> optionsBuilder;
        public DbContextFactory(IConfiguration configuration)
        {
            this.optionsBuilder = new DbContextOptionsBuilder<TrailsAppContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            this.optionsBuilder.UseSqlServer(connectionString);
        }

        /// <inheritdoc/>
        public TrailsAppContext CreateContext()
        {
            return new TrailsAppContext(this.optionsBuilder.Options, ConfigurationsAssembly);
        }

        /// <inheritdoc/>
        public TrailsAppContext CreateReadOnlyContext()
        {
            var context = new TrailsAppContext(this.optionsBuilder.Options, ConfigurationsAssembly);
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return context;
        }
    }
}
