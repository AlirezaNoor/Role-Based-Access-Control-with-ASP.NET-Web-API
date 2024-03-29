﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoleBasedAuthSample.Models;
using RoleBasedAuthSample.Services;
using System.IdentityModel.Tokens.Jwt;
using RoleBasedAuthSample.Dto;
using RoleBasedAuthSample.Extension;

namespace RoleBasedAuthSample.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: auth/login
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginUser user)
        {
            // Error checks

            if (String.IsNullOrEmpty(user.UserName))
            {
                return BadRequest(new { message = "User name needs to entered" });
            }
            else if (String.IsNullOrEmpty(user.Password))
            {
                return BadRequest(new { message = "Password needs to entered" });
            }

            // Try login

            var loggedInUser = await _authService.Login(new User(user.UserName, "", user.Password, null,0));

            // Return responses

            if (loggedInUser != null)
            {
                return Ok(loggedInUser);
            }

            return BadRequest(new { message = "User login unsuccessful" });
        }

        // POST: auth/register
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUser user)
        {
            // Error checks

            if (String.IsNullOrEmpty(user.Name))
            {
                return BadRequest(new { message = "Name needs to entered" });
            }
            else if (String.IsNullOrEmpty(user.UserName))
            {
                return BadRequest(new { message = "User name needs to entered" });
            }
            else if (String.IsNullOrEmpty(user.Password))
            {
                return BadRequest(new { message = "Password needs to entered" });
            }

            // Try registration

            var registeredUser = await _authService.Register(new User(user.UserName, user.Name, user.Password, user.Roles,user.AccessId));

            // Return responses

            if (registeredUser != null)
            {
                return Ok(registeredUser);
            }

            return BadRequest(new { message = "User registration unsuccessful" });
        }

        // GET: auth/test
        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Test()
        {
            // Get token from header

            string token = Request.Headers["Authorization"];

            if (token.StartsWith("Bearer"))
            {
                token = token.Substring("Bearer ".Length).Trim();
            }
            var handler = new JwtSecurityTokenHandler();

            // Returns all claims present in the token

            JwtSecurityToken jwt = handler.ReadJwtToken(token);

            var claims = "List of Claims: \n\n";

            foreach (var claim in jwt.Claims)
            {
                claims += $"{claim.Type}: {claim.Value}\n";
            }

            return Ok(claims);
        }
        
 
        [HttpGet]
        public async Task<ReponseDto> Test2()
        {
            
          var result=await  _authService.GetbyUserName(User.Identity.Name);
            // Get token from header
            var isAdmin = UserRoleChecker.CheckUserRole(User,3);
            string token = Request.Headers["Authorization"];
            if (isAdmin==false)
            {
                return new ReponseDto()
                {
                    IsSuccess = false,
                    Message = "Access denied!"

                };
            }
            else
            {
                if (token.StartsWith("Bearer"))
                {
                    token = token.Substring("Bearer ".Length).Trim();
                }
                var handler = new JwtSecurityTokenHandler();

                // Returns all claims present in the token

                JwtSecurityToken jwt = handler.ReadJwtToken(token);

                var claims = "List of Claims: \n\n";

                foreach (var claim in jwt.Claims)
                {
                    claims += $"{claim.Type}: {claim.Value}\n";
                }

                return new ReponseDto()
                {
                    IsSuccess = true,
                    Message = "this is added",
                    Data = claims
                };
            }
         
        }
    }
}
