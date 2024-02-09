using ExpectativaMercadoMensais.Application.Services;
using ExpectativaMercadoMensais.CrossCutting.Mapper;
using ExpectativaMercadoMensais.Domain.Dtos;
using ExpectativaMercadoMensais.Domain.Entities;
using ExpectativaMercadoMensais.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Net.Http;


namespace ExpectativaMercadoMensais.Teste
{
    public class ExpectativaMercadoMensalAppServiceTests
    {

        private Mock<IHttpClientFactory> _httpClientFactoryMock;
        private Mock<IMapperService> _mapperServiceMock;
        private Mock<HttpClient> _httpServiceMock;
        private ExpectativaMercadoMensalAppService _service;


        [SetUp]
        public void Setup()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _mapperServiceMock = new Mock<IMapperService>();
            _httpServiceMock = new Mock<HttpClient>();

            _service = new ExpectativaMercadoMensalAppService(_mapperServiceMock.Object, _httpServiceMock.Object);
        }

        [Test]
        public async Task GetAllTipoIndicador_ReturnsTipoIndicador()
        {
            var tipoIndicadores = new List<string> { "IPCA", "IGPM", "SELIC" };

            var result = await _service.GetAllTipoIndicador();

            Assert.NotNull(result);
            Assert.AreEqual(tipoIndicadores.Count, result.Count());
            foreach (var tipoIndicador in tipoIndicadores)
            {
                Assert.IsTrue(result.Any(t => t.Equals(tipoIndicador)));
            }
        }


        [Test]
        public async Task GetAllTipoIndicador_ShouldReturnTipos()
        {
            var expected = new List<string> { "IPCA", "IGPM", "SELIC" };

            var result = await _service.GetAllTipoIndicador();

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetAllTipoIndicador_ShouldReturnExpectedTypes()
        {
            var expected = new List<string> { "IPCA", "IGPM", "SELIC" };

            var result = await _service.GetAllTipoIndicador();

            result.Should().BeEquivalentTo(expected, options =>
                options.WithStrictOrdering());
        }

        [Test]
        public async Task GetAllTipoIndicador_ShouldReturnEmptyList_WhenServiceThrowsException()
        {

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .Throws<TaskCanceledException>();

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);


            var result = await _service.GetAllTipoIndicador();

            result.Should().BeEmpty();
        }

        [Test]
        public async Task GetAllExpectativasMercadoMensal_ShouldReturnNull_WhenNoData()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("[]", Encoding.UTF8, "application/json")
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            var result = await _service.GetExpectativasMercadoMensalAsync(string.Empty);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetAllExpectativasMercadoMensal_ShouldReturnNull_WhenInvalidData()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("invalid data", Encoding.UTF8, "application/json")
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);
            
            var result = await _service.GetExpectativasMercadoMensalAsync(string.Empty);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetAllExpectativasMercadoMensal_Returns_Expected_Data()
        {

            var _mapperService = new MapperService();
            var httpClient = new HttpClient();
            var expectativaMercadoMensalAppService = new ExpectativaMercadoMensalAppService(_mapperService, httpClient); 


            var response = await expectativaMercadoMensalAppService.GetExpectativasMercadoMensalAsync(string.Empty);

            Assert.IsNotNull(response);
            Assert.IsTrue(response.Any());
            
        }

        public async Task GetAllExpectativasMercadoMensal_Should_Return_Expectativas_Mercado_Mensal()
        {
            var expectativasMensais = new List<ExpectativaMercadoMensal>
            {
                new ExpectativaMercadoMensal
                {
                    Indicador = "IPCA",
                    Data = new DateTime(2022, 01, 01),
                    DataReferencia = "01/2022",
                    Media = 0.534,
                    Mediana = 0.53,
                    DesvioPadrao = 0.058,
                    Minimo = 0.428,
                    Maximo = 0.6,
                    NumeroRespondentes = 13,
                    BaseCalculo = 1
                },
                new ExpectativaMercadoMensal
                {
                    Indicador = "IPCA",
                    Data = new DateTime(2022, 02, 01),
                    DataReferencia = "02/2022",
                    Media = 0.552,
                    Mediana = 0.55,
                    DesvioPadrao = 0.048,
                    Minimo = 0.48,
                    Maximo = 0.6,
                    NumeroRespondentes = 12,
                    BaseCalculo = 1
                }
            };

            var responseString = JsonConvert.SerializeObject(expectativasMensais);
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseString)
            };

            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>()).GetAsync(It.IsAny<string>()))
                .ReturnsAsync(response);

            _mapperServiceMock.Setup(_ => _.ExpectativaMercadoMensalResponseToExpectativaMercadoMensal(It.IsAny<ExpectativaMercadoMensalResponse>()))
                .Returns(expectativasMensais);

            var result = await _service.GetExpectativasMercadoMensalAsync(string.Empty);

            Assert.IsNotNull(result);
            Assert.IsAssignableFrom<IEnumerable<ExpectativaMercadoMensal>>(result);
            Assert.AreEqual(expectativasMensais.Count, result.Count());
            foreach (var expectativa in expectativasMensais)
            {
                Assert.IsTrue(result.Any(e => e.Indicador == expectativa.Indicador && e.Data == expectativa.Data));
            }
        }


    }
}