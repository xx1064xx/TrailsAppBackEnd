
using TrailsAppRappi.Core;

namespace TrailsAppRappi.Interfaces
{
    public interface IDbContextFactory
    {

        TrailsAppContext CreateContext();

        TrailsAppContext CreateReadOnlyContext();

    }
}
