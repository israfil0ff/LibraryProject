using AutoMapper;
using Library.BLL.Exceptions;
using Library.BLL.Helpers;
using Library.BLL.Interfaces;
using Library.DAL.Context;
using Library.DBO;
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

        public FeedbackService(LibraryDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void Add(FeedbackCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Comment))
                throw new AppException(ErrorCode.InvalidFeedbackInput);

            var feedback = _mapper.Map<Feedback>(dto);
            _context.Feedbacks.Add(feedback);
            _context.SaveChanges();
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

            return new PaginationResponse<FeedbackGetDto>(items, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
