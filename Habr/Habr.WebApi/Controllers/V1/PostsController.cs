using Habr.BL.Interfaces.Services.V1;
using Habr.Common.DTOs.V1.Posts;
using Habr.WebApi.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Habr.WebApi.Controllers.V1;

[ApiVersion("1.0")]
[ApiVersion("2.0")]
[ApiVersion("3.0")]
public class PostsController : BaseController
{
    private readonly IPostService _postService;

    public PostsController(IPostService postService)
    {
        _postService = postService;
    }
    
    [HttpGet]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllPosts()
    {
        return Ok(await _postService.GetAllPostsAsync());
    }

    [HttpGet("public")]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllPublicPosts()
    {
        return Ok(await _postService.GetAllPublicPostsAsync());
    }
    
    [HttpGet("public/author/{authorId}")]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPublicPostsByUser([FromRoute] int authorId)
    {
        return Ok(await _postService.GetPublicPostsByUserAsync(authorId));
    }
    
    [HttpGet("draft")]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllDraftPosts()
    {
        return Ok(await _postService.GetAllDraftPostsAsync());
    }
    
    [HttpGet("draft/author/{authorId}")]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDraftPostsByUser([FromRoute] int authorId)
    {
        return Ok(await _postService.GetDraftPostsByUserAsync(authorId));
    }
    
    [HttpGet("{id}")]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPostById([FromRoute] int id)
    {
        return Ok(await _postService.GetPostByIdAsync(id));
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddPost([FromBody] AddPostRequest addPostRequest)
    {
        var response = await _postService.AddPostAsync(addPostRequest, UserId);
        
        return CreatedAtAction(nameof(GetPostById), new { id = response.Id }, response);
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdatePost(
        [FromRoute] int id, 
        [FromBody] UpdatePostRequest updatePostRequest
    )
    {
        await _postService.UpdatePostAsync(id, updatePostRequest, UserId);

        return NoContent();
    }
    
    [HttpPut("other/{id}")]
    [Authorize(Policy = PolicyNames.RequireAdministratorRole)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateOtherPost(
        [FromRoute] int id, 
        [FromBody] UpdatePostRequest updatePostRequest
    )
    {
        await _postService.UpdatePostAsync(id, updatePostRequest);

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeletePost([FromRoute] int id)
    {
        await _postService.DeletePostAsync(id, UserId);
        
        return NoContent();
    }
    
    [HttpDelete("other/{id}")]
    [Authorize(Policy = PolicyNames.RequireAdministratorRole)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteOtherPost([FromRoute] int id)
    {
        await _postService.DeletePostAsync(id);
        
        return NoContent();
    }
    
    [HttpPost("{id}/publish")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> PublishPost([FromRoute] int id)
    {
        await _postService.PublishPostAsync(id, UserId);
        
        return Accepted();
    }
    
    [HttpPost("{id}/moveToDraft")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> MoveToDraft([FromRoute] int id)
    {
        await _postService.MoveToDraftAsync(id, UserId);

        return Accepted();
    }
}