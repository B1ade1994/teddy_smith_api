using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using teddy_smith_api.Dtos.Comment;
using teddy_smith_api.Interfaces;
using teddy_smith_api.Mappers;

namespace teddy_smith_api.Controllers
{
  [Route("api/comments")]
  [ApiController]
  public class CommentController : ControllerBase
  {
    private readonly ICommentRepository _commentRepo;
    private readonly IStockRepository _stockRepo;

    public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
    {
      _commentRepo = commentRepo;
      _stockRepo = stockRepo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      var comments = await _commentRepo.GetAllAsync();

      var commentDto = comments.Select(s => s.ToCommentDto());

      return Ok(commentDto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
      var comment = await _commentRepo.GetByIdAsync(id);

      if (comment == null)
        return NotFound();

      return Ok(comment.ToCommentDto());
    }

    [HttpPost("{stockId}")]
    public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentDto commentDto)
    {
      if (!await _stockRepo.StockExists(stockId))
      {
        return BadRequest("Stock does not exists");
      }

      var comment = commentDto.ToCommentFromCreate(stockId);
      await _commentRepo.CreateAsync(comment);

      return CreatedAtAction(nameof(GetById), new { id = comment }, comment.ToCommentDto());
    }
  }
}