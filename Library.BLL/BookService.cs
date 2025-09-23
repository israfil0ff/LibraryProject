using AutoMapper;
using AutoMapper.QueryableExtensions;
using Library.DAL.Repositories;
using Library.DBO;
using Library.DBO.Pagination;
using Library.Entities;
using Library.Entities.Enums;
using Library.BLL.Exceptions;
using Library.BLL.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.BLL.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public BookService(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        // =======================
        // Pagination + filter
        // =======================
        public PaginationResponse<BookGetDTO> GetAll(PaginationRequest request, Dictionary<string, string>? filters = null)
        {
            var query = _bookRepository.GetQueryable();

            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    if (filter.Key.Equals("title", System.StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(filter.Value))
                        query = query.Where(b => b.Title.Contains(filter.Value));

                    if (filter.Key.Equals("authorName", System.StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(filter.Value))
                        query = query.Where(b => b.Author.Name.Contains(filter.Value));
                }
            }

            var totalCount = query.Count();

            var items = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ProjectTo<BookGetDTO>(_mapper.ConfigurationProvider)
                .ToList();

            return new PaginationResponse<BookGetDTO>(items, totalCount, request.PageNumber, request.PageSize);
        }

        public BookGetDTO? GetById(int id)
        {
            var book = _bookRepository.GetById(id);
            if (book == null) return null;

            return new BookGetDTO
            {
                Id = book.Id,
                Title = book.Title,
                isDeleted = book.IsDeleted,
                AuthorId = book.AuthorId,
                AuthorName = book.Author?.Name,
                CategoryId = book.CategoryId,
                CategoryName = book.Category?.Name,
                AvailableCount = book.AvailableCount
            };
        }

        public int Add(BookCreateDto dto)
        {
            if (dto.CategoryId != null && !_bookRepository.CategoryExists(dto.CategoryId.Value))
                throw new AppException(ErrorCode.CategoryNotFound);

            var book = _mapper.Map<Book>(dto);
            _bookRepository.Add(book);
            _bookRepository.Save();

            return book.Id;
        }

        public int Update(BookUpdateDto dto)
        {
            var book = _bookRepository.GetById(dto.Id)
                ?? throw new AppException(ErrorCode.BookNotFound);

            if (dto.CategoryId != null && !_bookRepository.CategoryExists(dto.CategoryId.Value))
                throw new AppException(ErrorCode.CategoryNotFound);

            _mapper.Map(dto, book);
            _bookRepository.Update(book);
            _bookRepository.Save();

            return book.Id;
        }

        public bool Delete(int id)
        {
            var book = _bookRepository.GetById(id)
                ?? throw new AppException(ErrorCode.BookNotFound);

            _bookRepository.Delete(book);
            _bookRepository.Save();

            return true;
        }

        public bool AddCount(int bookId, int count)
        {
            var book = _bookRepository.GetById(bookId)
                ?? throw new AppException(ErrorCode.BookNotFound);

            book.AvailableCount += count;
            _bookRepository.Update(book);
            _bookRepository.Save();

            return true;
        }

        // =======================
        // AI üçün əlavə metodlar
        // =======================
        public async Task<IEnumerable<BookGetDTO>> GetAllBooksAsync()
        {
            var books = await _bookRepository.GetAllAsync();
            return books.Select(b => new BookGetDTO
            {
                Id = b.Id,
                Title = b.Title,
                isDeleted = b.IsDeleted,
                AuthorId = b.AuthorId,
                AuthorName = b.Author?.Name,
                CategoryId = b.CategoryId,
                CategoryName = b.Category?.Name,
                AvailableCount = b.AvailableCount
            });
        }

        public async Task<IEnumerable<BookGetDTO>> GetBooksByTopicAsync(string topic)
        {
            var books = await _bookRepository.GetAllAsync();
            var filtered = books.Where(b => b.Title.Contains(topic) || b.Description.Contains(topic));
            return filtered.Select(b => new BookGetDTO
            {
                Id = b.Id,
                Title = b.Title,
                isDeleted = b.IsDeleted,
                AuthorId = b.AuthorId,
                AuthorName = b.Author?.Name,
                CategoryId = b.CategoryId,
                CategoryName = b.Category?.Name,
                AvailableCount = b.AvailableCount
            });
        }
    }
}
