using AutoMapper;
using ExpectativaMercadoMensais.CrossCutting.Mapper.Profile;
using ExpectativaMercadoMensais.Domain.Dtos;
using ExpectativaMercadoMensais.Domain.Entities;
using ExpectativaMercadoMensais.Domain.Interfaces;

namespace ExpectativaMercadoMensais.CrossCutting.Mapper
{
    public class MapperService : IMapperService
    {

        private IMapper _iMapper;

        public MapperService()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ExpectativaMercadoMensalProfile>();
            });

            _iMapper = config.CreateMapper();
        }

        public IEnumerable<ExpectativaMercadoMensal> ExpectativaMercadoMensalResponseToExpectativaMercadoMensal(ExpectativaMercadoMensalResponse expectativaMercadoMensalResponse)
        {
            return _iMapper.Map<IEnumerable<ExpectativaMercadoMensal>>(expectativaMercadoMensalResponse);
        }

    }
}
