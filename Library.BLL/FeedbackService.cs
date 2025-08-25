using AutoMapper;
using Library.BLL.Interfaces;
using Library.DAL.Context;
using Library.DBO;
using Library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var feedback = _mapper.Map<Feedback>(dto);
            _context.Feedbacks.Add(feedback);
            _context.SaveChanges();
        }

        public IEnumerable<FeedbackGetDto> GetAll()
        {
            return _mapper.Map<IEnumerable<FeedbackGetDto>>(_context.Feedbacks.ToList());
        }
    }
}
