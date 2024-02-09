using AutoMapper;
using ExpectativaMercadoMensais.Domain.Dtos;
using ExpectativaMercadoMensais.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpectativaMercadoMensais.CrossCutting.Mapper.Profile
{
    public class ExpectativaMercadoMensalProfile : AutoMapper.Profile
    {
        public ExpectativaMercadoMensalProfile()
        {

            CreateMap<ExpectativaMercadoMensalResponse, IEnumerable<ExpectativaMercadoMensal>>()
                .ConvertUsing(src => src.Itens.Select(dto => new ExpectativaMercadoMensal
                {
                    Indicador = dto.Indicador,
                    Data = dto.Data,
                    DataReferencia = dto.DataReferencia,
                    Media = (double)dto.Media,
                    Mediana = (double)dto.Mediana,
                    DesvioPadrao = (double)dto.DesvioPadrao,
                    Minimo = (double)dto.Minimo,
                    Maximo = (double)dto.Maximo,
                    NumeroRespondentes = dto.NumeroRespondentes,
                    BaseCalculo = dto.BaseCalculo
                }));

            CreateMap<ExpectativaMercadoMensalDto, ExpectativaMercadoMensal>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data))
                .ForMember(dest => dest.Media, opt => opt.MapFrom(src => src.Media))
                .ForMember(dest => dest.Mediana, opt => opt.MapFrom(src => src.Mediana))
                .ForMember(dest => dest.DesvioPadrao, opt => opt.MapFrom(src => src.DesvioPadrao))
                .ForMember(dest => dest.Minimo, opt => opt.MapFrom(src => src.Minimo))
                .ForMember(dest => dest.Maximo, opt => opt.MapFrom(src => src.Maximo))
                .ForMember(dest => dest.NumeroRespondentes, opt => opt.MapFrom(src => src.NumeroRespondentes))
                .ForMember(dest => dest.BaseCalculo, opt => opt.MapFrom(src => src.BaseCalculo));
        }

    }
}
