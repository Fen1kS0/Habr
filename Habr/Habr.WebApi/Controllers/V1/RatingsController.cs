using Habr.BL.Interfaces.Services.V1;
using Habr.Common.DTOs.V1.Rating;
using Microsoft.AspNetCore.Mvc;

namespace Habr.WebApi.Controllers.V1;

[ApiVersion("1.0")]
[ApiVersion("2.0")]
[ApiVersion("3.0")]
[Route("api/v{version:apiVersion}/post/{postId:int}/[controller]")]
public class RatingsController : BaseController
{
    private readonly IRatingService _ratingService;

    public RatingsController(IRatingService ratingService)
    {
        _ratingService = ratingService;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRatings([FromRoute] int postId)
    {
        return Ok(await _ratingService.GetAllRatings(postId));
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRatingById([FromRoute] int postId, int id)
    {
        return Ok(await _ratingService.GetRatingById(postId, id));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddRating([FromRoute] int postId, [FromBody] AddRatingRequest addRatingRequest)
    {
        var response = await _ratingService.AddRating(postId, addRatingRequest, UserId);
        
        return CreatedAtAction(nameof(GetRatingById), new { postId = response.PostId, id = response.Id }, response);
    } 
}