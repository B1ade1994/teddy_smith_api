using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace teddy_smith_api.Models
{
  public class User : IdentityUser
  {
    public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
  }
}