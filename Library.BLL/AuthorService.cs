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

    public List<AuthorDto> GetAll()
    {
        var authors = _context.Authors
            .Include(a => a.Books)
            .Where(a => !a.IsDeleted) 
            .ToList();

        return _mapper.Map<List<AuthorDto>>(authors);
    }

    public AuthorDto? GetById(int id)
    {
        var author = _context.Authors.Include(a => a.Books).FirstOrDefault(a => a.Id == id);
        return author == null ? null : _mapper.Map<AuthorDto>(author);
    }

    public void Add(AuthorCreateDto authorDto)
    {
        var author = _mapper.Map<Author>(authorDto);
        _context.Authors.Add(author);
        _context.SaveChanges();
    }

    public void Update(AuthorUpdateDto authorDto)
    {
        var existingAuthor = _context.Authors.Find(authorDto.Id);
        if (existingAuthor == null) return;

        _mapper.Map(authorDto, existingAuthor);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var author = _context.Authors.FirstOrDefault(a => a.Id == id);
        if (author == null)
            throw new Exception("Author not found");

        author.IsDeleted = true; 
        _context.SaveChanges();
    }



