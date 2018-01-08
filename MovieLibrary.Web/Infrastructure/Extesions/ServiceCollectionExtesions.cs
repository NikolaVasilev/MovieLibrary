using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MovieLibrary.Services;

namespace MovieLibrary.Web.Infrastructure.Extesions
{
    public static class ServiceCollectionExtesions
    {
        public static IServiceCollection AddDomainService(this IServiceCollection services)
        {
            Assembly
                .GetAssembly(typeof(IService))
                .GetTypes()
                .Where(t => t.IsClass && t.GetInterfaces().Any(i => i.Name == $"I{t.Name}"))
                .Select(t => new
                {
                    Interface = t.GetInterface($"I{t.Name}"),
                    Implementation = t
                }).ToList().ForEach(s => services.AddTransient(s.Interface, s.Implementation));

            return services;

        }
    }
}
