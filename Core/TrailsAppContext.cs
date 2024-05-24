using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TrailsAppRappi.Core.Entities;

namespace TrailsAppRappi.Core
{
    public class TrailsAppContext : DbContext
    {

        private readonly Assembly entityConfigurationAssembly;

        public TrailsAppContext(DbContextOptions<TrailsAppContext> options, Assembly entityConfigurationsAssemly)
            : base(options)
        {
            this.entityConfigurationAssembly = entityConfigurationsAssemly;
        }

        public virtual DbSet<Trail> Trails { get; set; }
        public virtual DbSet<User> Users { get; set; }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            // Automatically Add all entity configurations
            modelBuilder.ApplyConfigurationsFromAssembly(this.entityConfigurationAssembly, type => type.Namespace?.EndsWith(".EntityConfigurations", StringComparison.Ordinal) == true);

            // Remove OnDeleteCascade from all Foreign Keys
            var foreignKeys = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);
            foreach (var fk in foreignKeys)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }


    }
}
