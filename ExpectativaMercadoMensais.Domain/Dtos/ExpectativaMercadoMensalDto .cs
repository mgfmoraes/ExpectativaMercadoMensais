using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpectativaMercadoMensais.Domain.Dtos
{
    public class ExpectativaMercadoMensalDto
    {
        [JsonProperty("Indicador")]
        public string Indicador { get; set; }

        [JsonProperty("Data")]
        public DateTime Data { get; set; }

        [JsonProperty("DataReferencia")]
        public string DataReferencia { get; set; }

        [JsonProperty("Media")]
        public decimal Media { get; set; }

        [JsonProperty("Mediana")]
        public decimal Mediana { get; set; }

        [JsonProperty("DesvioPadrao")]
        public decimal DesvioPadrao { get; set; }

        [JsonProperty("Minimo")]
        public decimal Minimo { get; set; }

        [JsonProperty("Maximo")]
        public decimal Maximo { get; set; }

        [JsonProperty("numeroRespondentes")]
        public int? NumeroRespondentes { get; set; }

        [JsonProperty("baseCalculo")]
        public int BaseCalculo { get; set; }
    }
}
