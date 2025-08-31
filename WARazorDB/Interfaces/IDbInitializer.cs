using Microsoft.Extensions.DependencyInjection;
using System;

namespace WARazorDB.Interfaces
{
    public abstract class IDbInitializer
    {
        // M�todo abstracto que debe implementar cada seeder
        public abstract void Initialize(IServiceProvider serviceProvider);
    }
}