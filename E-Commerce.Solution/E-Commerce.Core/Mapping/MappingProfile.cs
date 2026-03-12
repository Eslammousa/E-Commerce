using AutoMapper;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.DTO.AdressDTO;
using E_Commerce.Core.DTO.CategoryDTO;
using E_Commerce.Core.DTO.OrderDTO;
using E_Commerce.Core.DTO.ProductDTO;
using E_Commerce.Core.DTO.ReviewDTO;

namespace E_Commerce.Core.Mapping
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {


            CreateMap<CategoryAddRequest, Category>();

            CreateMap<CategoryUpdateRequest, Category>();

            CreateMap<Category, CategoryResponse>();


            CreateMap<ProductAddRequest, Product>();
            CreateMap<ProudctUpdateRequest, Product>();

            CreateMap<Product, ProductResponse>()
             .ForMember(d => d.CategoryName,
                       opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<Product, ResponseProductWithReview>()
                .ForMember(d => d.Reviews,
                        opt => opt.MapFrom(src => src.Reviews));




            CreateMap<Order, OrderResponse>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.TotalPrice,
                    opt => opt.MapFrom(src => src.OrderItems
                        .Sum(i => i.UnitPrice * i.Quantity)))
                .ForMember(dest => dest.ShippingAddress,
                    opt => opt.MapFrom(src => new ResponseAdress
                    {
                        Id = src.AddressId,
                        PersonName = src.PersonName,
                        Phone = src.Phone,
                        Country = src.ShippingCountry,
                        City = src.ShippingCity,
                        Street = src.ShippingStreet,
                        Building = src.ShippingBuilding
                    }));

            CreateMap<OrderItem, OrderItemResponse>()
                .ForMember(dest => dest.ProductName,
                    opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.UnitPrice,
                    opt => opt.MapFrom(src => src.UnitPrice))
                .ForMember(dest => dest.ImageUrl,
                    opt => opt.MapFrom(src => src.Product.Image)) 
                .ForMember(dest => dest.TotalPrice,
                    opt => opt.MapFrom(src => src.UnitPrice * src.Quantity));


            CreateMap<Address, ResponseAdress>();


            CreateMap<ReviewAddRequest, Review>();
            CreateMap<Review, ReviewResponse>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.User.PersonName));






        }
    }
}
