using AutoMapper;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Domain.RepositoryContracts;
using E_Commerce.Core.DTO.ReviewDTO;
using E_Commerce.Core.Exceptions;
using E_Commerce.Core.ServicesContracts;

namespace E_Commerce.Core.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Review> _reviewRepository;
        private readonly IGenericRepository<Product> _ProudctRepo;
        private readonly ICurrentUserService _currentUserService;
        public ReviewService(IUnitOfWork unitOfWork, IMapper mapper, IGenericRepository<Review> reviewRepository
            , ICurrentUserService currentUserService, IGenericRepository<Product> proudctRepo)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _reviewRepository = reviewRepository;
            _currentUserService = currentUserService;
            _ProudctRepo = proudctRepo;
        }
        public async Task<ReviewResponse> AddReviewAsync(Guid ProductId, ReviewAddRequest reviewAddRequest)
        {
            var userId = _currentUserService.UserId;

            var product = await _ProudctRepo.FindAsync(x => x.Id == ProductId);
            if (product == null) throw new EntityNotFoundException("Product not found");


            var existingReview = await _reviewRepository.FindAsync(x => x.UserId == userId && x.ProductId == ProductId);
            if (existingReview != null) throw new InvalidOperationException("User has already reviewed this product");

            var review = _mapper.Map<Review>(reviewAddRequest);
            review.UserId = userId;
            review.ProductId = ProductId;

            var oldCount = product.ReviewCount;
            var oldAverage = product.AvgRating;

            product.ReviewCount++;

            product.AvgRating = ((oldAverage * oldCount) + review.Rating) / product.ReviewCount;

            await _reviewRepository.AddAsync(review);
            await _unitOfWork.SaveAsync();

            var existingReviewWithUser = await _reviewRepository.FindAsync
                (x => x.Id == review.Id, x => x.User);


            return _mapper.Map<ReviewResponse>(existingReviewWithUser);

        }

        public async Task<List<ReviewResponse>> GetAllReviewsByProudct(Guid ProductId)
        {
            var reviews = await _reviewRepository.WhereAsync(x => x.ProductId == ProductId, x => x.User);
            return _mapper.Map<List<ReviewResponse>>(reviews);

        }
    }
}