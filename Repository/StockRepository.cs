using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using teddy_smith_api.Data;
using teddy_smith_api.Interfaces;
using teddy_smith_api.Models;
using Microsoft.EntityFrameworkCore;
using teddy_smith_api.Dtos.Stock;

namespace teddy_smith_api.Repository
{
  public class StockRepository : IStockRepository
  {
    private readonly ApplicationDbContext _context;

    public StockRepository(ApplicationDbContext context)
    {
      _context = context;
    }

    public async Task<Stock> CreateAsync(Stock stock)
    {
      await _context.Stocks.AddAsync(stock);
      await _context.SaveChangesAsync();

      return stock;
    }

    public async Task<Stock?> DeleteAsync(int id)
    {
      var stock = await _context.Stocks.FindAsync(id);

      if (stock == null)
        return null;

      _context.Stocks.Remove(stock);
      await _context.SaveChangesAsync();

      return stock;
    }

    public async Task<List<Stock>> GetAllAsync()
    {
      return await _context.Stocks.Include(c => c.Comments).ToListAsync();
    }

    public async Task<Stock?> GetByIdAsync(int id)
    {
      return await _context.Stocks.FindAsync(id);
    }

    public Task<bool> StockExists(int id)
    {
      return _context.Stocks.AnyAsync(s => s.Id == id);
    }

    public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
    {
      // Find не работает с Include
      var stock = await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(i => i.Id == id);

      if (stock == null)
        return null;

      stock.Symbol = stockDto.Symbol;
      stock.CompanyName = stockDto.CompanyName;
      stock.Purchase = stockDto.Purchase;
      stock.LastDiv = stockDto.LastDiv;
      stock.Industry = stockDto.Industry;
      stock.MarketCap = stockDto.MarketCap;

      await _context.SaveChangesAsync();

      return stock;
    }
  }
}