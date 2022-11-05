using Habr.BL.Interfaces.Services.V2;
using Microsoft.AspNetCore.Mvc;

namespace Habr.WebApi.Controllers.V2;

[ApiVersion("2.0")]
[ApiVersion("3.0")]
public class PostsController : BaseController
{
    private readonly IPostServiceV2 _postService;

    public PostsController(IPostServiceV2 postService)
    {
        _postService = postService;
    }
    
    [HttpGet]
    [MapToApiVersion("2.0")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllPosts()
    {
        return Ok(await _postService.GetAllPostsAsync());
    }

    [HttpGet("public")]
    [MapToApiVersion("2.0")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllPublicPosts()
    {
        return Ok(await _postService.GetAllPublicPostsAsync());
    }
    
    [HttpGet("public/author/{authorId}")]
    [MapToApiVersion("2.0")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPublicPostsByUser([FromRoute] int authorId)
    {
        return Ok(await _postService.GetPublicPostsByUserAsync(authorId));
    }
    
    [HttpGet("draft")]
    [MapToApiVersion("2.0")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllDraftPosts()
    {
        return Ok(await _postService.GetAllDraftPostsAsync());
    }
    
    [HttpGet("draft/author/{authorId}")]
    [MapToApiVersion("2.0")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDraftPostsByUser([FromRoute] int authorId)
    {
        return Ok(await _postService.GetDraftPostsByUserAsync(authorId));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPostById([FromRoute] int id)
    {
        return Ok(await _postService.GetPostByIdAsync(id));
    }
}