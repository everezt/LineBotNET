using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using lineBot.Core.ServiceInterfaces.ApplicationServices;
using Microsoft.Extensions.DependencyInjection;

namespace lineBot.ApplicationServices.Ext
{
    public static class ServicesExt
    {
        public static void AddAppServices(this IServiceCollection services, Assembly assembly)
        {
            services.AddServicesFromAssembly<IApplicationService>(assembly);
        }

        public static IServiceCollection AddServicesFromAssembly<TBase>(this IServiceCollection services, Assembly assembly)
        {
            var baseType = typeof(TBase);

            var regCollection = assembly.GetTypes()
                .Where(baseType.IsAssignableFrom)
                .Where(x => !x.IsAbstract)
                .Select(x =>
                {
                    var allInterfaces = x.GetInterfaces();
                    var interfaces = allInterfaces
                        .Where(i => baseType.IsAssignableFrom(i))
                        .Except(allInterfaces.SelectMany(t => t.GetInterfaces()));

                    return new { implementation = x, services = interfaces };
                }).ToArray();

            foreach (var reg in regCollection)
            {
                foreach (var service in reg.services)
                {
                    var s = service;
                    var i = reg.implementation;

                    if (service.IsGenericType)
                    {
                        s = service.GetGenericTypeDefinition();
                    }

                    if (reg.implementation.IsGenericType)
                    {
                        i = reg.implementation.GetGenericTypeDefinition();
                    }

                    services.AddScoped(s, i);
                }
            }

            return services;
        }
    }
}
