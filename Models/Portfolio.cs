using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace teddy_smith_api.Models
{
  [Table("Portfolios")]
  public class Portfolio
  {
    public string UserId { get; set; }
    public int StockId { get; set; }
    public User User { get; set; }
    public Stock Stock { get; set; }
  }
}