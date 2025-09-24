using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using teddy_smith_api.Models;

namespace teddy_smith_api.Interfaces
{
  public interface IPortolioRepository
  {
    Task<List<Stock>> GetUserPortfolio(User user);
    Task<Portfolio> CreateAsync(Portfolio portfolio);
    Task<Portfolio> DeleteAsync(User user, string symbol);
  }
}