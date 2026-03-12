using E_Commerce.Core.DTO.ReviewDTO;
using E_Commerce.Core.ServicesContracts;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost("{ProductId:guid}")]
        public async Task<ActionResult> AddReview([FromRoute] Guid ProductId, [FromBody] ReviewAddRequest reviewAddRequest)
        {
            var result = await _reviewService.AddReviewAsync(ProductId, reviewAddRequest);
            return Ok(result);
        }

        [HttpDelete("{ReviewId:guid}")]
        public async Task<ActionResult> DeleteReview([FromRoute] Guid ReviewId)
        {
            await _reviewService.DeleteReview(ReviewId);
            return NoContent();
        }



    }
}