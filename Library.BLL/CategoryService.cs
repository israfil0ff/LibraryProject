using AutoMapper;
using AutoMapper.QueryableExtensions;
using Library.BLL.Exceptions;
using Library.BLL.Helpers;
using Library.BLL.Interfaces;
using Library.DAL.Context;
using Library.DBO;
using Library.DBO.Pagination;
using Library.Entities;
using Library.Entities.Enums;
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
                throw new AppException(ErrorCode.InvalidCategoryInput);

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


        public PaginationResponse<CategoryDto> GetAll(PaginationRequest request, Dictionary<string, string>? filters = null)
        {
            var query = _context.Categories
                .Where(c => !c.IsDeleted)
                .AsQueryable();


            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    if (filter.Key.Equals("name", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(filter.Value))
                        query = query.Where(c => c.Name.Contains(filter.Value));
                }
            }

            var totalCount = query.Count();

            var items = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                .ToList();

            return new PaginationResponse<CategoryDto>(items, totalCount, request.PageNumber, request.PageSize);
        }


        public PaginationResponse<CategoryWithBooksDto> GetAllWithBooks(PaginationRequest request, Dictionary<string, string>? filters = null)
        {
            var query = _context.Categories
                .Where(c => !c.IsDeleted)
                .Include(c => c.Books)
                .AsQueryable();

            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    if (filter.Key.Equals("name", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(filter.Value))
                        query = query.Where(c => c.Name.Contains(filter.Value));
                }
            }

            var totalCount = query.Count();

            var items = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ProjectTo<CategoryWithBooksDto>(_mapper.ConfigurationProvider)
                .ToList();

            return new PaginationResponse<CategoryWithBooksDto>(items, totalCount, request.PageNumber, request.PageSize);
        }

        public CategoryDto GetById(int id)
        {
            var category = _context.Categories
                .Include(c => c.Books)
                .FirstOrDefault(c => c.Id == id && !c.IsDeleted)
                ?? throw new AppException(ErrorCode.CategoryNotFound);

            return _mapper.Map<CategoryDto>(category);
        }
    }
}
