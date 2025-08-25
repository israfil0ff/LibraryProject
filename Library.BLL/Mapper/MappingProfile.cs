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
            CreateMap<Category, CategoryDto>()
    .ForMember(dest => dest.Books, opt => opt.MapFrom(src => src.Books));
            CreateMap<Book, BookShortDto>();

            CreateMap<Category, CategoryWithBooksDto>()
                .ForMember(d => d.Books,
                    opt => opt.MapFrom(s => s.Books
                        .Where(b => !b.IsDeleted)   
                        .Select(b => b)             
                    ));

            CreateMap<Author, AuthorGetDTO>()
        .ForMember(dest => dest.BookTitles, opt => opt.MapFrom(src => src.Books.Select(b => b.Title)));

            CreateMap<AuthorCreateDto, Author>();
            CreateMap<AuthorUpdateDto, Author>();

            CreateMap<Book, BookGetDTO>()
    .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null));


            CreateMap<BookCreateDto, Book>();
            CreateMap<BookUpdateDto, Book>();
            CreateMap<Book, BookSummaryDto>();

            CreateMap<BookRental, BookRentalDto>();
            CreateMap<BookRentalDto, BookRental>();
            CreateMap<FeedbackCreateDto, Feedback>();
            CreateMap<Feedback, FeedbackGetDto>();
        }
    }
}


