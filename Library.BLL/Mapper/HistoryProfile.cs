using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Library.DBO.HistoryDTOs;
using Library.Entities;

namespace Library.BLL
{
    public class HistoryProfile : Profile
    {
        public HistoryProfile()
        {
            
            CreateMap<History, HistoryReadDTO>();

            
            CreateMap<HistoryCreateDTO, History>();
        }
    }
}

