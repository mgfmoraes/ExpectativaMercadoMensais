using ExpectativaMercadoMensais.Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpectativaMercadoMensais.Domain.Dtos
{
    public class ExpectativaMercadoMensalResponse
    {
        [JsonProperty("@odata.context")]
        public string Context { get; set; }

        [JsonProperty("value")]
        public List<ExpectativaMercadoMensalDto> Itens { get; set; }
    }
}
