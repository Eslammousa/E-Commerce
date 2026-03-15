using AutoMapper;
using E_Commerce.Core.Common;
using E_Commerce.Core.Domain.RepositoryContracts;
using E_Commerce.Core.DTO;
using E_Commerce.Core.DTO.AdressDTO;
using E_Commerce.Core.DTO.CategoryDTO;
using E_Commerce.Core.ServicesContracts;

namespace E_Commerce.Core.Services
{
    public class AdressService : IAdressService
    {
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        public AdressService(IMapper mapper
            , ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }
        public async Task<PagedResult<ResponseAdress>> GetAllAdressesAsync(PaginationDTO paginationDTO)
        {
            var userId = _currentUserService.UserId;

            var (items, totalCount) = await _unitOfWork.Addresses.WhereAsync
                (x => x.UserId == userId,
                 sortBy: paginationDTO.SortBy,
                     sortDirection: paginationDTO.sortDirection,
                     pageNumber: paginationDTO.Page,
                     pageSize: paginationDTO.Size

                );

            return new PagedResult<ResponseAdress>
            {
                Items = _mapper.Map<IEnumerable<ResponseAdress>>(items),
                TotalCount = totalCount,
                PageNumber = paginationDTO.Page,
                PageSize = paginationDTO.Size,
                TotalPages = (int)Math.Ceiling(totalCount / (double)paginationDTO.Size)
            };

        }
    }
}