using AutoMapper;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.DTO.CartDTO;
using E_Commerce.Core.DTO.CategoryDTO;
using E_Commerce.Core.DTO.OrderDTO;
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

            CreateMap<CartItem, CartItemResponse>()
                .ForMember(d => d.ProductName,
                    opt => opt.MapFrom(src => src.Product.Name));

            CreateMap<Cart, CartResponse>()
                .ForMember(d => d.CartItems,
                    opt => opt.MapFrom(src => src.CartItems));

            CreateMap<Order, OrderResponse>()
     .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => src.Status.ToString()))
     .ForMember(dest => dest.OrderItems,
                opt => opt.MapFrom(src => src.OrderItems));

            CreateMap<OrderItem, OrderItemResponse>()
       .ForMember(dest => dest.ProductName,
           opt => opt.MapFrom(src => src.Product.Name))
       .ForMember(dest => dest.ImageUrl,
           opt => opt.MapFrom(src =>
              src.Product.Image))
       .ForMember(dest => dest.UnitPrice,
           opt => opt.MapFrom(src => src.UnitPrice));







        }
    }
}
