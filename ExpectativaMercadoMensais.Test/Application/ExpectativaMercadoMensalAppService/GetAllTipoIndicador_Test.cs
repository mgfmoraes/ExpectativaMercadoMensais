using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpectativaMercadoMensais.Application.Services;
using ExpectativaMercadoMensais.Domain.Interfaces;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;


namespace ExpectativaMercadoMensais.Test.Application.ExpectativaMercadoMensalAppService
{
    public class GetAllTipoIndicador_Test
    {
        private readonly Mock<ILogger<ExpectativaMercadoMensais.Application.Services.ExpectativaMercadoMensalAppService>> _loggerMock;
        private readonly ExpectativaMercadoMensais.Application.Services.ExpectativaMercadoMensalAppService _service;

        public GetAllTipoIndicador_Test()
        {
            _loggerMock = new Mock<ILogger<ExpectativaMercadoMensais.Application.Services.ExpectativaMercadoMensalAppService>>();
            //_service = new ExpectativaMercadoMensais.Application.Services.ExpectativaMercadoMensalAppService(_loggerMock.Object);
            _service = new ExpectativaMercadoMensais.Application.Services.ExpectativaMercadoMensalAppService();
        }

        [Fact]
        public void GetAllTipoIndicador_NotNull()
        {
            var expectativaMercadoMensalAppService = new ExpectativaMercadoMensais.Application.Services.ExpectativaMercadoMensalAppService();
            var tiposExpectativa = expectativaMercadoMensalAppService.GetAllTipoIndicador();
            Assert.NotNull(tiposExpectativa);
        }

        //[Fact]
        //public void GetAllTipoIndicador_Return_Data()
        //{
        //    var expectativaMercadoMensalAppService = new ExpectativaMercadoMensais.Application.Services.ExpectativaMercadoMensalAppService();
        //    var tiposExpectativa = expectativaMercadoMensalAppService.GetAllTipoIndicador();
        //    Assert.Contains(tiposExpectativa.Result.);
        //}

        [Fact]
        public async Task GetAllTipoIndicador_ReturnsTipoIndicador()
        {
            // Arrange
            var tipoIndicadores = new List<string> { "IPCA", "IGPM", "SELIC" };

            // Act
            var result = await _service.GetAllTipoIndicador();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(tipoIndicadores.Count, result.Count());
            foreach (var tipoIndicador in tipoIndicadores)
            {
                Assert.Contains(result, t => t == tipoIndicador);
            }
        }

    }
}
