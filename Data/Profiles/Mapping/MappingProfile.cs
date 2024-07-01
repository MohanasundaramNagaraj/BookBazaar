using AutoMapper;
using BookBazaar.Entities;
using BookBazaar.Profiles.Dtos;

namespace BookBazaar.Profiles.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to Dto
            CreateMap<Book, BookDto>();
            CreateMap<Category, CategoryDto>();
            CreateMap<Cover, CoverDto>();
            CreateMap<Author, AuthorDto>();
            CreateMap<ApplicationUser, ApplicationUserDto>();
            CreateMap<Company, CompanyDto>();
            CreateMap<ShoppingCart, ShoppingCartDto>();
            CreateMap<OrderHeader, OrderHeaderDto>();
            CreateMap<OrderDetail, OrderDetailDto>();


            // Dto to Domain
            CreateMap<BookDto, Book>()
                .ForMember(p => p.Id, opt => opt.Ignore());

            CreateMap<CategoryDto, Category>()
                 .ForMember(p => p.Id, opt => opt.Ignore());

            CreateMap<CoverDto, Cover>()
                .ForMember(p => p.Id, opt => opt.Ignore());

            CreateMap<AuthorDto, Author>()
                 .ForMember(p => p.Id, opt => opt.Ignore());

            CreateMap<ApplicationUserDto, ApplicationUser>()
                 .ForMember(p => p.Id, opt => opt.Ignore());

            CreateMap<CompanyDto, Company>()
                 .ForMember(p => p.Id, opt => opt.Ignore());

            CreateMap<ShoppingCartDto, ShoppingCart>()
                 .ForMember(p => p.Id, opt => opt.Ignore());

            CreateMap<OrderHeaderDto, OrderHeader>()
                .ForMember(p => p.Id, opt => opt.Ignore());

            CreateMap<OrderDetailDto, OrderDetail>()
                .ForMember(p => p.Id, opt => opt.Ignore());
        }
    }
}
