using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using teddy_smith_api.Extensions;
using teddy_smith_api.Interfaces;
using teddy_smith_api.Models;

namespace teddy_smith_api.Controllers
{
  [Route("api/portfolio")]
  [Authorize]
  [ApiController]
  public class PortfolioController : ControllerBase
  {
    private readonly UserManager<User> _userManager;
    private readonly IStockRepository _stockRepo;
    private readonly IPortolioRepository _portfolioRepo;

    public PortfolioController(UserManager<User> userManager, IStockRepository stockRepo, IPortolioRepository portfolioRepo)
    {
      _userManager = userManager;
      _stockRepo = stockRepo;
      _portfolioRepo = portfolioRepo;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserPortfolio()
    {
      var username = User.GetUsername();
      var user = await _userManager.FindByNameAsync(username);
      var userPortfolio = await _portfolioRepo.GetUserPortfolio(user);

      return Ok(userPortfolio);
    }

    [HttpPost]
    public async Task<IActionResult> AddPortfolio(string symbol)
    {
      var username = User.GetUsername();
      var user = await _userManager.FindByNameAsync(username);
      var stock = await _stockRepo.GetBySymbolAsync(symbol);

      if (stock == null)
        return BadRequest("Stock not found");

      var userPortfolio = await _portfolioRepo.GetUserPortfolio(user);

      if (userPortfolio.Any(p => p.Symbol.ToLower() == symbol.ToLower()))
        return BadRequest("Stock already in portfolio");

      var portfolioModel = new Portfolio
      {
        UserId = user.Id,
        StockId = stock.Id
      };

      await _portfolioRepo.CreateAsync(portfolioModel);

      if (portfolioModel == null)
        return StatusCode(500, "Could not create");

      return Created();
    }

    [HttpDelete]
    public async Task<IActionResult> DeletePortfolio(string symbol)
    {
      var username = User.GetUsername();
      var user = await _userManager.FindByNameAsync(username);
      var userPortfolio = await _portfolioRepo.GetUserPortfolio(user);

      var filteredStock = userPortfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower()).ToList();

      if (filteredStock.Count == 1)
      {
        await _portfolioRepo.DeleteAsync(user, symbol);
      }
      else
      {
        return BadRequest("Stock not in your portfolio");
      }

      return Ok();
    }
  }
}