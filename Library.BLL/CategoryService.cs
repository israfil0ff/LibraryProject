using AutoMapper;
using Library.BLL.Exceptions;
using Library.BLL.Helpers;
using Library.BLL.Interfaces;
using Library.DAL.Context;
using Library.DBO;
using Library.Entities;
using Library.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Library.BLL
{
    public class CategoryService : ICategoryService
    {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;

        public CategoryService(LibraryDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public int Add(CategoryCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new AppException(ErrorCode.InvalidCategoryInput);

            if (_context.Categories.Any(c => c.Name == dto.Name && !c.IsDeleted))
                throw new AppException(ErrorCode.InvalidCategoryInput); 

            var category = _mapper.Map<Category>(dto);
            _context.Categories.Add(category);
            _context.SaveChanges();

            return category.Id;
        }

        public int Update(CategoryUpdateDto dto)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == dto.Id && !c.IsDeleted)
                ?? throw new AppException(ErrorCode.CategoryNotFound);

            if (_context.Categories.Any(c => c.Name == dto.Name && c.Id != dto.Id && !c.IsDeleted))
                throw new AppException(ErrorCode.InvalidCategoryInput); // artıq mövcuddur

            category.Name = dto.Name;
            _context.SaveChanges();

            return category.Id;
        }

        public bool Delete(int id)
        {
            var category = _context.Categories
                .Include(c => c.Books)
                .FirstOrDefault(c => c.Id == id && !c.IsDeleted)
                ?? throw new AppException(ErrorCode.CategoryNotFound);

            if (category.Books != null)
            {
                foreach (var book in category.Books)
                    book.CategoryId = null;
            }

            category.IsDeleted = true;
            _context.SaveChanges();

            return true;
        }

        public List<CategoryDto> GetAll()
            => _context.Categories
                .Where(c => !c.IsDeleted)
                .ToList()
                .Select(c => _mapper.Map<CategoryDto>(c))
                .ToList();

        public CategoryDto GetById(int id)
        {
            var category = _context.Categories
                .Include(c => c.Books)
                .FirstOrDefault(c => c.Id == id && !c.IsDeleted)
                ?? throw new AppException(ErrorCode.CategoryNotFound);

            return _mapper.Map<CategoryDto>(category);
        }

        public List<CategoryWithBooksDto> GetAllWithBooks()
        {
            var categories = _context.Categories
                .Where(c => !c.IsDeleted)
                .Include(c => c.Books)
                .ToList();

            return _mapper.Map<List<CategoryWithBooksDto>>(categories);
        }
    }
}
