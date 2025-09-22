using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using teddy_smith_api.Data;
using teddy_smith_api.Mappers;
using teddy_smith_api.Dtos.Stock;
using teddy_smith_api.Interfaces;
using teddy_smith_api.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace teddy_smith_api.Controllers
{
  [Route("api/stocks")]
  [Authorize]
  [ApiController]
  public class StockController : ControllerBase
  {
    private readonly ApplicationDbContext _context;
    private readonly IStockRepository _stockRepo;

    public StockController(ApplicationDbContext context, IStockRepository stockRepo)
    {
      _stockRepo = stockRepo;
      _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
    {
      var stocks = await _stockRepo.GetAllAsync(query);
      var stockDto = stocks.Select(s => s.ToStockDto());

      return Ok(stockDto);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
      var stock = await _stockRepo.GetByIdAsync(id);

      if (stock == null)
        return NotFound();

      return Ok(stock.ToStockDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
    {
      var stock = stockDto.ToStockFromCreateDto();
      await _stockRepo.CreateAsync(stock);
      return CreatedAtAction(nameof(GetById), new { id = stock.Id }, stock.ToStockDto());
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto stockDto)
    {
      var stock = await _stockRepo.UpdateAsync(id, stockDto);

      if (stock == null)
        return NotFound();

      return Ok(stock.ToStockDto());
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
      var stock = await _stockRepo.DeleteAsync(id);

      if (stock == null)
        return NotFound();

      return NoContent();
    }
  }
}