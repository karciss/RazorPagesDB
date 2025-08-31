using Microsoft.Extensions.DependencyInjection;
using System;

namespace WARazorDB.Interfaces
{
    public abstract class IDbInitializer
    {
        // Método abstracto que debe implementar cada seeder
        public abstract void Initialize(IServiceProvider serviceProvider);
    }
}