using AutoMapper;
using Library.BLL.Interfaces;
using Library.DAL.Repositories;
using Library.DBO.HistoryDTOs;
using Library.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Library.BLL.Services
{
    public class HistoryService : IHistoryService
    {
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;

        public HistoryService(IHistoryRepository historyRepository, IMapper mapper)
        {
            _historyRepository = historyRepository;
            _mapper = mapper;
        }

        public void AddHistory(HistoryCreateDTO dto)
        {
            var entity = _mapper.Map<History>(dto);
            _historyRepository.Add(entity);
            _historyRepository.Save();
        }

        public IEnumerable<HistoryReadDTO> GetAll()
        {
            var list = _historyRepository.GetAll();
            return _mapper.Map<IEnumerable<HistoryReadDTO>>(list);
        }

        public HistoryReadDTO? GetById(int id)
        {
            var entity = _historyRepository.GetById(id);
            if (entity == null)
                return null;

            return _mapper.Map<HistoryReadDTO>(entity);
        }

        public IEnumerable<HistoryReadDTO> GetByEntity(string entityName, int entityId)
        {
            var list = _historyRepository
                .GetAll()
                .Where(h => h.EntityName == entityName && h.EntityId == entityId)
                .ToList();

            return _mapper.Map<IEnumerable<HistoryReadDTO>>(list);
        }
    }
}
