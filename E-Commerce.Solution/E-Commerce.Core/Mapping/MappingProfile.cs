using AutoMapper;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.DTO.CategoryDTO;
using E_Commerce.Core.DTO.ProductDTO;

namespace E_Commerce.Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapping AddRequestCategory to Person
            CreateMap<CategoryAddRequest, Category>();

            // Mapping UpdateRequestCategory to Person
            CreateMap<CategoryUpdateRequest, Category>();

            // Mapping Id == gg // Convert Person to PersonDTO
            CreateMap<Category, CategoryResponse>();


            CreateMap<ProductAddRequest, Product>();
            CreateMap<ProudctUpdateRequest, Product>();

            CreateMap<Product, ProductResponse>()
             .ForMember( d => d.CategoryName,
                       opt => opt.MapFrom(src => src.Category.Name));

        }
    }
}
