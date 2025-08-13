using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using Library.Entities;
using Library.DBO;

namespace Library.BLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<CategoryUpdateDto, Category>();
            CreateMap<Category, CategoryDto>();

            CreateMap<Author, AuthorDto>()
        .ForMember(dest => dest.BookTitles, opt => opt.MapFrom(src => src.Books.Select(b => b.Title)));

            CreateMap<AuthorCreateDto, Author>();
            CreateMap<AuthorUpdateDto, Author>();

            CreateMap<Book, BookDto>()
    .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null));


            CreateMap<BookCreateDto, Book>();
            CreateMap<BookUpdateDto, Book>();

            CreateMap<BookRental, BookRentalDto>();
            CreateMap<BookRentalDto, BookRental>();
        }
    }
}


