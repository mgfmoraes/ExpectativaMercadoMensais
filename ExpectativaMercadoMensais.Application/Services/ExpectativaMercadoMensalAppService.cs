using ExpectativaMercadoMensais.Domain.Dtos;
using ExpectativaMercadoMensais.Domain.Entities;
using ExpectativaMercadoMensais.Domain.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ExpectativaMercadoMensais.Application.Services
{
    public class ExpectativaMercadoMensalAppService : IExpectativaMercadoMensalAppService
    {

        private readonly IMapperService _mapperService;
        private readonly HttpClient _httpClient;

        public ExpectativaMercadoMensalAppService(IMapperService mapperService, HttpClient httpClient)
        {
            _mapperService = mapperService;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ExpectativaMercadoMensal>> GetExpectativasMercadoMensalAsync(string tipoIndicador)
        {

            try
            {
                var filter = string.Empty;
                if (!string.IsNullOrEmpty(tipoIndicador))
                {
                    filter = $"&%24filter=Indicador eq '{tipoIndicador}'";
                }
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                var uri = new Uri($"https://olinda.bcb.gov.br/olinda/servico/Expectativas/versao/v1/odata/ExpectativaMercadoMensais?%24format=json&%24top=1000{filter}");

                IEnumerable<ExpectativaMercadoMensal> expectativas = null;

                var uriBuilder = new UriBuilder(uri);
                var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
                using (var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri))
                {
                    var responseMessage = _httpClient.Send(request);

                    responseMessage.EnsureSuccessStatusCode();

                    var json = await responseMessage.Content.ReadAsStringAsync();
                    var dados = JsonConvert.DeserializeObject<ExpectativaMercadoMensalResponse>(json);

                    expectativas = _mapperService.ExpectativaMercadoMensalResponseToExpectativaMercadoMensal(dados);

                }
                return expectativas;


            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Erro ao obter as expectativas de mercado mensais: " + ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao obter as expectativas de mercado mensais: " + ex.Message);
                return null;
            }

        }

        public async Task<IEnumerable<string>> GetAllTipoIndicador()
        {
            var tipo = new List<string>();

            tipo.Add("IPCA");
            tipo.Add("IGP-M");
            tipo.Add("Selic");

            return tipo.AsEnumerable();
        }
    }
}
