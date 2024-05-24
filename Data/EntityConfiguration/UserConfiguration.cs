namespace TrailsAppRappi.Data.EntityConfiguration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TrailsAppRappi.Core.Entities;

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.UserId);
        }
    }
}
