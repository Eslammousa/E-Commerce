using AutoMapper;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.DTO.CategoryDTO;

namespace E_Commerce.Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapping AddRequestCategory to Person
            CreateMap<CategoryAddRequest, Category>();
           //.ForMember(
           //    dest => dest.Id,
           //    src => src.MapFrom(src => Guid.NewGuid())
           //);
            // Mapping UpdateRequestCategory to Person
            CreateMap<CategoryUpdateRequest, Category>();

            // Mapping Id == gg // Convert Person to PersonDTO
            CreateMap<Category, CategoryResponse>();//.ForMember(
           //    dest => dest.gg,
           //    src => src.MapFrom(src => src.Id)
           //);
        }
    }
}
