namespace TrailsAppRappi.Data.EntityConfiguration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TrailsAppRappi.Core.Entities;

    public class TrailConfiguration : IEntityTypeConfiguration<Trail>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<Trail> builder)
        {
            builder.HasKey(x => x.TrailId);
        }
    }
}