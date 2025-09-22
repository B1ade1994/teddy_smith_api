using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using teddy_smith_api.Dtos.Account;
using teddy_smith_api.Interfaces;
using teddy_smith_api.Models;

namespace teddy_smith_api.Controllers
{
  [Route("api/account")]
  [ApiController]
  public class AccountController : ControllerBase
  {
    public readonly UserManager<User> _userManager;
    public readonly ITokenService _tokenService;

    public AccountController(UserManager<User> userManager, ITokenService tokenService)
    {
      _userManager = userManager;
      _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
      try
      {
        if (!ModelState.IsValid)
          return BadRequest(ModelState);

        var user = new User
        {
          UserName = registerDto.Username,
          Email = registerDto.Email
        };

        var createdUser = await _userManager.CreateAsync(user, registerDto.Password);

        if (createdUser.Succeeded)
        {
          var roleResult = await _userManager.AddToRoleAsync(user, "User");

          if (roleResult.Succeeded)
          {
            return Ok(
              new NewUserDto
              {
                UserName = user.UserName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
              }
            );
          }
          else
          {
            return StatusCode(500, roleResult.Errors);
          }
        }
        else
        {
          return StatusCode(500, createdUser.Errors);
        }
      }
      catch (Exception e)
      {
        return StatusCode(500, e);
      }
    }
  }
}