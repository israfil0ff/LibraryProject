using AutoMapper;
using AutoMapper.QueryableExtensions;
using Library.DAL.Context;
using Library.Entities;
using Library.DBO;
using Microsoft.EntityFrameworkCore;
using Library.BLL.Exceptions;
using Library.Entities.Enums;

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
        => _context.Authors
            .Include(a => a.Books)
            .Where(a => !a.IsDeleted)
            .ProjectTo<AuthorGetDTO>(_mapper.ConfigurationProvider)
            .ToList();

    public AuthorGetDTO? GetById(int id)
    {
        var author = _context.Authors
            .Include(a => a.Books)
            .Where(a => a.Id == id && !a.IsDeleted)
            .ProjectTo<AuthorGetDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefault();

        if (author == null)
            throw new AppException(ErrorCode.AuthorNotFound);

        return author;
    }

    public int Add(AuthorCreateDto authorDto)
    {
        if (string.IsNullOrWhiteSpace(authorDto.Name))
            throw new AppException(ErrorCode.InvalidAuthorInput);

        var author = _mapper.Map<Author>(authorDto);
        _context.Authors.Add(author);
        _context.SaveChanges();

        return author.Id;
    }

    public int Update(AuthorUpdateDto authorDto)
    {
        var author = _context.Authors.FirstOrDefault(a => a.Id == authorDto.Id && !a.IsDeleted)
            ?? throw new AppException(ErrorCode.AuthorNotFound);

        _mapper.Map(authorDto, author);
        _context.SaveChanges();

        return author.Id;
    }

    public bool Delete(int id)
    {
        var author = _context.Authors.FirstOrDefault(a => a.Id == id && !a.IsDeleted)
            ?? throw new AppException(ErrorCode.AuthorNotFound);

        author.IsDeleted = true;
        _context.SaveChanges();

        return true;
    }
}
