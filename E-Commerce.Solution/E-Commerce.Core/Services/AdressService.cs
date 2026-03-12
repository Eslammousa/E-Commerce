using AutoMapper;
using E_Commerce.Core.Domain.RepositoryContracts;
using E_Commerce.Core.DTO.AdressDTO;
using E_Commerce.Core.Exceptions;
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
        public async Task<List<ResponseAdress>> GetAllAdressesAsync()
        {
            var userId = _currentUserService.UserId;
            var (Address,_) = await _unitOfWork.Addresses.WhereAsync(x => x.UserId == userId);

            if (Address == null || !Address.Any()) throw new EntityNotFoundException($"Adress not found");

            return _mapper.Map<List<ResponseAdress>>(Address);

        }
    }
}