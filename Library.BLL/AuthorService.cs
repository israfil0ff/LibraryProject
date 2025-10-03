using AutoMapper;
using AutoMapper.QueryableExtensions;
using Library.DAL.Context;
using Library.Entities;
using Library.DBO;
using Library.DBO.Pagination;
using Microsoft.EntityFrameworkCore;
using Library.BLL.Exceptions;
using Library.Entities.Enums;
using Library.BLL.Interfaces;
using Library.DBO.HistoryDTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.BLL;

public class AuthorService : IAuthorService
{
    private readonly LibraryDbContext _context;
    private readonly IMapper _mapper;
    private readonly IHistoryService _historyService;

    public AuthorService(LibraryDbContext context, IMapper mapper, IHistoryService historyService)
    {
        _context = context;
        _mapper = mapper;
        _historyService = historyService;
    }

    // ============================
    // Pagination + filter
    // ============================
    public PaginationResponse<AuthorGetDTO> GetAll(PaginationRequest request, Dictionary<string, string>? filters = null)
    {
        var query = _context.Authors
            .Include(a => a.Books)
            .Where(a => !a.IsDeleted)
            .AsQueryable();

        if (filters != null)
        {
            foreach (var filter in filters)
            {
                if (filter.Key.Equals("name", System.StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(filter.Value))
                    query = query.Where(a => a.Name.Contains(filter.Value));
            }
        }

        var totalCount = query.Count();

        var items = query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(a => new AuthorGetDTO
            {
                Id = a.Id,
                Name = a.Name,
                isDeleted = a.IsDeleted,
                BookTitles = a.Books.Count > 0
                    ? a.Books.Where(b => !b.IsDeleted).Select(b => b.Title).ToList()
                    : new List<string>()
            })
            .ToList();

        return new PaginationResponse<AuthorGetDTO>(items, totalCount, request.PageNumber, request.PageSize);
    }

    // ============================
    // Get by Id
    // ============================
    public AuthorGetDTO? GetById(int id)
    {
        var author = _context.Authors
            .Include(a => a.Books)
            .Where(a => a.Id == id && !a.IsDeleted)
            .Select(a => new AuthorGetDTO
            {
                Id = a.Id,
                Name = a.Name,
                isDeleted = a.IsDeleted,
                BookTitles = a.Books.Count > 0
                    ? a.Books.Where(b => !b.IsDeleted).Select(b => b.Title).ToList()
                    : new List<string>()
            })
            .FirstOrDefault();

        if (author == null)
            throw new AppException(ErrorCode.AuthorNotFound);

        return author;
    }

    // ============================
    // Add author
    // ============================
    public int Add(AuthorCreateDto authorDto)
    {
        if (string.IsNullOrWhiteSpace(authorDto.Name))
            throw new AppException(ErrorCode.InvalidAuthorInput);

        var author = new Author
        {
            Name = authorDto.Name
        };

        _context.Authors.Add(author);
        _context.SaveChanges();

       
        _historyService.AddHistory(new HistoryCreateDTO
        {
            EntityName = nameof(Author),
            EntityId = author.Id,
            Action = "Create",
            Status = "Success",
            Message = $"Author '{author.Name}' created.",
            NewValue = $"Name: {author.Name}",
            CreatedBy = "System" 
        });

        return author.Id;
    }

    // ============================
    // Update author
    // ============================
    public int Update(AuthorUpdateDto authorDto)
    {
        var author = _context.Authors.FirstOrDefault(a => a.Id == authorDto.Id && !a.IsDeleted)
            ?? throw new AppException(ErrorCode.AuthorNotFound);

        var oldName = author.Name;

        author.Name = authorDto.Name;
        _context.SaveChanges();

        
        _historyService.AddHistory(new HistoryCreateDTO
        {
            EntityName = nameof(Author),
            EntityId = author.Id,
            Action = "Update",
            Status = "Success",
            Message = $"Author '{oldName}' updated to '{author.Name}'.",
            OldValue = $"Name: {oldName}",
            NewValue = $"Name: {author.Name}",
            CreatedBy = "System"
        });

        return author.Id;
    }

    // ============================
    // Delete author
    // ============================
    public bool Delete(int id)
    {
        var author = _context.Authors.FirstOrDefault(a => a.Id == id && !a.IsDeleted)
            ?? throw new AppException(ErrorCode.AuthorNotFound);

        author.IsDeleted = true;
        _context.SaveChanges();

        
        _historyService.AddHistory(new HistoryCreateDTO
        {
            EntityName = nameof(Author),
            EntityId = author.Id,
            Action = "Delete",
            Status = "Success",
            Message = $"Author '{author.Name}' deleted.",
            OldValue = $"Name: {author.Name}",
            CreatedBy = "System"
        });

        return true;
    }

    // ============================
    // 🔹 Async metod
    // ============================
    public async Task<IEnumerable<AuthorGetDTO>> GetAllAuthorsAsync()
    {
        var authors = await _context.Authors
            .Include(a => a.Books)
            .Where(a => !a.IsDeleted)
            .ToListAsync();

        return authors.Select(a => new AuthorGetDTO
        {
            Id = a.Id,
            Name = a.Name,
            isDeleted = a.IsDeleted,
            BookTitles = a.Books?.Where(b => !b.IsDeleted).Select(b => b.Title).ToList()
        });
    }
}
