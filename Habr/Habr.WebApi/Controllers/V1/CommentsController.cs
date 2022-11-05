using Habr.BL.Interfaces.Services.V1;
using Habr.Common.DTOs.V1.Comments;
using Habr.WebApi.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Habr.WebApi.Controllers.V1;

[ApiVersion("1.0")]
[ApiVersion("2.0")]
[ApiVersion("3.0")]
[Route("api/v{version:apiVersion}/post/{postId:int}/[controller]")]
public class CommentsController : BaseController
{
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllComments([FromRoute] int postId)
    {
        return Ok(await _commentService.GetCommentsAsync(postId));
    }

    [HttpGet("author/{authorId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCommentsByUser([FromRoute] int postId, [FromRoute] int authorId)
    {
        return Ok(await _commentService.GetCommentsByUserAsync(postId, authorId));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCommentsById([FromRoute] int postId, [FromRoute] int id)
    {
        return Ok(await _commentService.GetCommentsByIdAsync(postId, id));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddComment(
        [FromRoute] int postId, 
        [FromBody] AddCommentRequest addCommentRequest
        )
    {
        var response = await _commentService.AddCommentAsync(postId, addCommentRequest, UserId);
        
        return CreatedAtAction(nameof(GetCommentsById), new { postId = response.PostId, id = response.Id }, response);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateComment(
        [FromRoute] int postId,
        [FromRoute] int id, 
        [FromBody] UpdateCommentRequest updateCommentRequest
    )
    {
        await _commentService.UpdateCommentAsync(postId, id, updateCommentRequest, UserId);
        
        return NoContent();
    }
    
    [HttpPut("other/{id}")]
    [Authorize(Policy = PolicyNames.RequireAdministratorRole)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateOtherComment(
        [FromRoute] int postId,
        [FromRoute] int id, 
        [FromBody] UpdateCommentRequest updateCommentRequest
    )
    {
        await _commentService.UpdateCommentAsync(postId, id, updateCommentRequest);
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteComment(
        [FromRoute] int postId, 
        [FromRoute] int id
    )
    {
        await _commentService.DeleteCommentAsync(postId, id, UserId);
        
        return NoContent();
    }
    
    [HttpDelete("other/{id}")]
    [Authorize(Policy = PolicyNames.RequireAdministratorRole)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteOtherComment(
        [FromRoute] int postId, 
        [FromRoute] int id
    )
    {
        await _commentService.DeleteCommentAsync(postId, id);
        
        return NoContent();
    }
}