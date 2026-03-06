using E_Commerce.Core.DTO.ReviewDTO;

namespace E_Commerce.Core.ServicesContracts
{
    public interface IReviewService
    {
        Task<ReviewResponse> AddReviewAsync(Guid ProudctId , ReviewAddRequest reviewAddRequest);
    }
}
