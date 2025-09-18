using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using teddy_smith_api.Dtos.Stock;
using teddy_smith_api.Helpers;
using teddy_smith_api.Models;

namespace teddy_smith_api.Interfaces
{
  public interface IStockRepository
  {
    Task<List<Stock>> GetAllAsync(QueryObject query);
    Task<Stock?> GetByIdAsync(int id);
    Task<Stock> CreateAsync(Stock stock);
    Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto);
    Task<Stock?> DeleteAsync(int id);
    Task<bool> StockExists(int id);
  }
}