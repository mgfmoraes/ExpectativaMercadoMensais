using ExpectativaMercadoMensais.CrossCutting.Ioc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.Data;
using System.Net.Http;
using System.Windows;

namespace ExpectativaMercadoMensais.WpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {

        private ServiceProvider serviceProvider;
        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
            this.Startup += OnStartup;
        }
        private void ConfigureServices(ServiceCollection services)
        {
            Injector.RegisterServices(services);
            services.AddSingleton<ExpectativaMercadoMensalViewModel>();
            services.AddSingleton<ExpectativaMercadoMensal>();
            services.AddHttpClient();
            //services.AddHttpClient("MyTypedClient")
            //    .ConfigurePrimaryHttpMessageHandler(() =>
            //    {
            //        var handler = new HttpClientHandler();

            //        // Configurar o registro de depuração
            //        handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            //        handler.ServerCertificateCustomValidationCallback =
            //            (httpRequestMessage, cert, cetChain, policyErrors) =>
            //            {
            //                // Implemente sua lógica de validação de certificado aqui
            //                return true;
            //            };

            //        return handler;
            //    })
            //    .AddHttpMessageHandler<ExpectativaMercadoMensalViewModel>();

            services.AddScoped<ExpectativaMercadoMensalViewModel>();
        }
        private void OnStartup(object sender, StartupEventArgs e)
        {
            //var mainWindow = serviceProvider.GetService<ExpectativaMercadoMensal>();
            //mainWindow.Show();
            var mainWindow = serviceProvider.GetService<ExpectativaMercadoMensal>();
            var viewModel = serviceProvider.GetService<ExpectativaMercadoMensalViewModel>();
            mainWindow.DataContext = viewModel;
            mainWindow.Show();
        }
    }

}
