using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tradify.Identity.Application.Mappings
{
    public class AssemblyMappingProfile : Profile
    {
        public AssemblyMappingProfile(Assembly assembly)
            => ApplyMappingsFromAssembly(assembly);

        // Method invoked in main. 
        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            // Get all types inheritors of IMappable
            var mappableTypes = assembly.GetExportedTypes()
                .Where(type => type.GetInterfaces()
                    .Any(i => i == typeof(IMappable)))
                .ToList();

            // Run through IMappable inheritors and invoke Mapping method
            foreach (var mappableType in mappableTypes)
            {
                var instance = Activator.CreateInstance(mappableType);

                var method = mappableType.GetMethod("Mapping");

                method?.Invoke(instance, new object?[] { this });
            }
        }
    }
}
