using ExpectativaMercadoMensais.Application.Services;
using ExpectativaMercadoMensais.CrossCutting.Mapper;
using ExpectativaMercadoMensais.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ExpectativaMercadoMensais.CrossCutting.Ioc
{
    public class Injector
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IExpectativaMercadoMensalAppService, ExpectativaMercadoMensalAppService>();
            services.AddScoped<HttpClient, HttpClient>();
            services.AddScoped<IMapperService, MapperService>();

        }
    }
}
