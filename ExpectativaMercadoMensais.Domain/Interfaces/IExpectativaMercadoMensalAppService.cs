using ExpectativaMercadoMensais.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpectativaMercadoMensais.Domain.Interfaces
{
    public interface IExpectativaMercadoMensalAppService
    {
        Task<IEnumerable<ExpectativaMercadoMensal>> GetExpectativasMercadoMensalAsync(string tipoIndicador);

        Task<IEnumerable<string>> GetAllTipoIndicador();
    }
}
