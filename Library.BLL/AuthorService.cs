using AutoMapper;
using Library.DAL.Context;
using Library.Entities;
using Library.DBO;
using Microsoft.EntityFrameworkCore;

namespace Library.BLL;

public class AuthorService : IAuthorService
{
    private readonly LibraryDbContext _context;
    private readonly IMapper _mapper;

    public AuthorService(LibraryDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public List<AuthorGetDTO> GetAll()
    {
        var authors = _context.Authors
            .Include(a => a.Books)
            .Where(a => !a.IsDeleted)
            .ToList();

        return _mapper.Map<List<AuthorGetDTO>>(authors);
    }

    public AuthorGetDTO? GetById(int id)
    {
        var author = _context.Authors
            .Include(a => a.Books)
            .FirstOrDefault(a => a.Id == id && !a.IsDeleted);

        return author == null ? null : _mapper.Map<AuthorGetDTO>(author);
    }

    public int Add(AuthorCreateDto authorDto)
    {
        var author = _mapper.Map<Author>(authorDto);
        _context.Authors.Add(author);
        _context.SaveChanges();

        return author.Id; 
    }

    public int Update(AuthorUpdateDto authorDto)
    {
        var existingAuthor = _context.Authors.FirstOrDefault(a => a.Id == authorDto.Id && !a.IsDeleted);
        if (existingAuthor == null) return 0;

        _mapper.Map(authorDto, existingAuthor);
        _context.SaveChanges();

        return existingAuthor.Id; 
    }

    public bool Delete(int id)
    {
        var author = _context.Authors.FirstOrDefault(a => a.Id == id && !a.IsDeleted);
        if (author == null) return false;

        author.IsDeleted = true;
        _context.SaveChanges();

        return true; 
    }
}
