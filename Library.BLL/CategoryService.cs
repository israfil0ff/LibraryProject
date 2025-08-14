using AutoMapper;
using Library.BLL.Interfaces;
using Library.DAL.Context;
using Library.DBO;
using Library.Entities;
using Microsoft.EntityFrameworkCore;
using System;
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

        
        public List<CategoryDto> GetAll()
        {
            var categories = _context.Categories
                .Where(c => !c.IsDeleted)
                .ToList();

            return _mapper.Map<List<CategoryDto>>(categories);
        }

        public CategoryDto GetById(int id)
        {
            var category = _context.Categories
        .Include(c => c.Books) 
        .FirstOrDefault(c => c.Id == id && !c.IsDeleted);

            if (category == null)
                return null;

            return _mapper.Map<CategoryDto>(category);
        }

        
        public void Add(CategoryCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new Exception("Ad boş ola bilməz.");

            if (_context.Categories.Any(c => c.Name == dto.Name && !c.IsDeleted))
                throw new Exception("Bu adla kateqoriya artıq mövcuddur.");

            var category = _mapper.Map<Category>(dto);
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        
        public void Update(CategoryUpdateDto dto)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == dto.Id && !c.IsDeleted);
            if (category == null)
                throw new Exception("Kateqoriya tapılmadı.");

            if (_context.Categories.Any(c => c.Name == dto.Name && c.Id != dto.Id && !c.IsDeleted))
                throw new Exception("Bu adla kateqoriya artıq mövcuddur.");

            category.Name = dto.Name;
            _context.SaveChanges();
        }
        public ApiResponse<List<CategoryWithBooksDto>> GetAllWithBooks()
        {
            try
            {
                var categories = _context.Categories
                    .Where(c => !c.IsDeleted)
                    .Include(c => c.Books)
                    .ToList();

                var data = _mapper.Map<List<CategoryWithBooksDto>>(categories);
                return ApiResponse<List<CategoryWithBooksDto>>
                    .SuccessResponse("Kateqoriyalar (+kitablar) siyahısı", data);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<CategoryWithBooksDto>>
                    .FailResponse("Xəta baş verdi: " + ex.Message);
            }
        }

        public void Delete(int id)
        {
            
            var category = _context.Categories
                .Include(c => c.Books)
                .FirstOrDefault(c => c.Id == id && !c.IsDeleted);

            if (category == null)
                throw new Exception("Kateqoriya tapılmadı.");

            
            if (category.Books != null && category.Books.Any())
            {
                foreach (var book in category.Books)
                {
                    book.CategoryId = null;
                }
            }

            category.IsDeleted = true;
            _context.SaveChanges();
        }
    }
}
