using AutoMapper;
using Library.BLL.Exceptions;
using Library.BLL.Helpers;
using Library.BLL.Interfaces;
using Library.DAL.Context;
using Library.DBO;
using Library.Entities;
using Library.Entities.Enums;
using System.Collections.Generic;
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

        public IEnumerable<FeedbackGetDto> GetAll()
        {
            var feedbacks = _context.Feedbacks.ToList();

            if (feedbacks == null || !feedbacks.Any())
                throw new AppException(ErrorCode.FeedbackNotFound);

            return _mapper.Map<IEnumerable<FeedbackGetDto>>(feedbacks);
        }
    }
}
