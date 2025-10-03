using AutoMapper;
using Library.BLL.Exceptions;
using Library.BLL.Helpers;
using Library.BLL.Interfaces;
using Library.DAL.Context;
using Library.DBO;
using Library.DBO.HistoryDTOs; 
using Library.DBO.Pagination;
using Library.Entities;
using Library.Entities.Enums;
using System.Linq;

namespace Library.BLL
{
    public class FeedbackService : IFeedbackService
    {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHistoryService _historyService;

        public FeedbackService(LibraryDbContext context, IMapper mapper, IHistoryService historyService)
        {
            _context = context;
            _mapper = mapper;
            _historyService = historyService;
        }

        public void Add(FeedbackCreateDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Comment))
                    throw new AppException(ErrorCode.InvalidFeedbackInput);

                var feedback = _mapper.Map<Feedback>(dto);
                _context.Feedbacks.Add(feedback);
                _context.SaveChanges();

                
                _historyService.AddHistory(new HistoryCreateDTO
                {
                    EntityName = "Feedback",
                    EntityId = feedback.Id,
                    Action = "Add",
                    Status = "Success",
                    Message = $"Feedback added: {feedback.Comment}"
                });
            }
            catch (Exception ex)
            {
                _historyService.AddHistory(new HistoryCreateDTO
                {
                    EntityName = "Feedback",
                    EntityId = 0,
                    Action = "Add",
                    Status = "Error",
                    Message = ex.Message
                });
                throw;
            }
        }

        public PaginationResponse<FeedbackGetDto> GetAll(PaginationRequest request)
        {
            var query = _context.Feedbacks.AsQueryable();

            var totalCount = query.Count();

            var items = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList()
                .Select(f => _mapper.Map<FeedbackGetDto>(f))
                .ToList();

            
            _historyService.AddHistory(new HistoryCreateDTO
            {
                EntityName = "Feedback",
                EntityId = 0,
                Action = "GetAll",
                Status = "Success",
                Message = $"Retrieved {items.Count} feedbacks"
            });

            return new PaginationResponse<FeedbackGetDto>(items, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
