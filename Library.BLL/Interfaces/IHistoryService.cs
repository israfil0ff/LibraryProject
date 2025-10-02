using Library.DBO.HistoryDTOs;
using System.Collections.Generic;

namespace Library.BLL.Interfaces
{
    public interface IHistoryService
    {
        void AddHistory(HistoryCreateDTO dto);
        IEnumerable<HistoryReadDTO> GetAll();
        HistoryReadDTO? GetById(int id);
        IEnumerable<HistoryReadDTO> GetByEntity(string entityName, int entityId);
    }
}
