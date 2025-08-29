using Library.DBO;
using Library.DBO.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.BLL.Interfaces
{
    public interface IFeedbackService
    {
        void Add(FeedbackCreateDto dto);
        PaginationResponse<FeedbackGetDto> GetAll(PaginationRequest request);
    }
}
