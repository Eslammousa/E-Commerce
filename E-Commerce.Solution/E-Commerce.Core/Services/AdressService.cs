using AutoMapper;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Domain.RepositoryContracts;
using E_Commerce.Core.DTO.AdressDTO;
using E_Commerce.Core.Exceptions;
using E_Commerce.Core.ServicesContracts;

namespace E_Commerce.Core.Services
{
    public class AdressService : IAdressService
    {
        private readonly IGenericRepository<Address> _adressRepo;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        public AdressService(IGenericRepository<Address> adressRepo, IMapper mapper
            , ICurrentUserService currentUserService)
        {
            _adressRepo = adressRepo;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        public async Task<List<ResponseAdress>> GetAllAdressesAsync()
        {
            var userId = _currentUserService.UserId;
            var Address =await _adressRepo.WhereAsync(x => x.UserId == userId);

            if(Address == null) throw new EntityNotFoundException($"Adress not found");

           return _mapper.Map<List<ResponseAdress>>(Address);

        }
    }
}