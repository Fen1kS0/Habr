using Habr.BL.Interfaces.Services.V3;
using Habr.Common.DTOs.Pages;
using Microsoft.AspNetCore.Mvc;

namespace Habr.WebApi.Controllers.V3;

[ApiVersion("3.0")]
public class PostsController : BaseController
{
    private readonly IPostServiceV3 _postService;

    public PostsController(IPostServiceV3 postService)
    {
        _postService = postService;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllPosts([FromQuery] PageParameters pageParameters)
    {
        return Ok(await _postService.GetAllPostsAsync(pageParameters));
    }

    [HttpGet("public")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllPublicPosts([FromQuery] PageParameters pageParameters)
    {
        return Ok(await _postService.GetAllPublicPostsAsync(pageParameters));
    }
    
    [HttpGet("public/author/{authorId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPublicPostsByUser([FromRoute] int authorId, [FromQuery] PageParameters pageParameters)
    {
        return Ok(await _postService.GetPublicPostsByUserAsync(authorId, pageParameters));
    }
    
    [HttpGet("draft")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllDraftPosts([FromQuery] PageParameters pageParameters)
    {
        return Ok(await _postService.GetAllDraftPostsAsync(pageParameters));
    }
    
    [HttpGet("draft/author/{authorId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDraftPostsByUser([FromRoute] int authorId, [FromQuery] PageParameters pageParameters)
    {
        return Ok(await _postService.GetDraftPostsByUserAsync(authorId, pageParameters));
    }
}