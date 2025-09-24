using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using teddy_smith_api.Interfaces;
using teddy_smith_api.Models;
using teddy_smith_api.Data;
using Microsoft.EntityFrameworkCore;
namespace teddy_smith_api.Repository
{
  public class PortfolioRepository : IPortolioRepository
  {
    private readonly ApplicationDbContext _context;

    public PortfolioRepository(ApplicationDbContext context)
    {
      _context = context;
    }

    public async Task<Portfolio> CreateAsync(Portfolio portfolio)
    {
      await _context.Portfolios.AddAsync(portfolio);
      await _context.SaveChangesAsync();
      return portfolio;
    }

    public async Task<Portfolio> DeleteAsync(User user, string symbol)
    {
      var portfolioModel = await _context.Portfolios.FirstOrDefaultAsync(x => x.UserId == user.Id && x.Stock.Symbol.ToLower() == symbol.ToLower());

      if (portfolioModel == null)
        return null;

      _context.Portfolios.Remove(portfolioModel);
      await _context.SaveChangesAsync();
      return portfolioModel;
    }

    public Task<List<Stock>> GetUserPortfolio(User user)
    {
      return _context.Portfolios.Where(u => u.UserId == user.Id).Select(stock => new Stock
      {
        Id = stock.StockId,
        Symbol = stock.Stock.Symbol,
        CompanyName = stock.Stock.CompanyName,
        Purchase = stock.Stock.Purchase,
        LastDiv = stock.Stock.LastDiv,
        Industry = stock.Stock.Industry,
        MarketCap = stock.Stock.MarketCap
      }).ToListAsync();
    }
  }
}