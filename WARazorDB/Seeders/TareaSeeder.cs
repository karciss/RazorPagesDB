using Bogus;
using WARazorDB.Data;
using WARazorDB.Interfaces;
using WARazorDB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace WARazorDB.Seeders
{
    public class TareaSeeder : IDbInitializer
    {
        public override void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<TareaDbContext>();
            context.Database.EnsureCreated();

            // Llama a la lógica de siembra de datos dentro de este método
            SeedTareaData(context);
        }

        private void SeedTareaData(TareaDbContext context)
        {
            context.Tareas.RemoveRange(context.Tareas);
            context.SaveChanges();

            // que cumplan con las validaciones (3-100 caracteres, sin caracteres especiales
            var nombreTareaGenerator = new Faker<string>("es")
                .CustomInstantiator(f =>
                {
                    string nombre;
                    do
                    {
                        // Generar un nombre de tarea que tenga entre 3 y 90 caracteres
                        // para dejar margen y evitar exceder el límite de 100
                        nombre = f.Lorem.Sentence(f.Random.Int(1, 5)).TrimEnd('.');
                        
                        // Si es muy largo, cortarlo
                        if (nombre.Length > 90)
                            nombre = nombre.Substring(0, 90);
                        
                        // Si es muy corto, añadir palabras
                        if (nombre.Length < 3)
                            nombre += " " + f.Lorem.Word();
                            
                    } while (nombre.Length < 3 || nombre.Length > 100 || 
                             ContainsProhibitedChars(nombre));
                    
                    return nombre;
                });
                
            string[] estadosPermitidos = { "Pendiente", "En Progreso", "Completado", "Cancelado" };

            // Crear una instancia de Faker para usar en la generación de fechas
            var faker = new Faker("es");

            // Genera una lista de 50 tareas de prueba
            var tareaFaker = new Faker<Tarea>("es")
                .RuleFor(t => t.nombreTarea, f => nombreTareaGenerator.Generate())
                .RuleFor(t => t.fechaVencimiento, f => f.Date.Between(DateTime.Today, DateTime.Today.AddYears(1)))
                .RuleFor(t => t.estado, f => f.PickRandom(estadosPermitidos))
                .RuleFor(t => t.idUsuario, f => f.Random.Number(1, 10)); // IDs de usuario entre 1 y 10

            var tareas = tareaFaker.Generate(50);

            context.Tareas.AddRange(tareas);
            context.SaveChanges();
        }
        
        private bool ContainsProhibitedChars(string input)
        {
            string[] prohibitedChars = { "@", "#", "$", "%", "&", "*", "^", "~", "`", "<", ">", "{", "}", "[", "]", "|", "\\", "/" };
            
            foreach (var character in prohibitedChars)
            {
                if (input.Contains(character))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
