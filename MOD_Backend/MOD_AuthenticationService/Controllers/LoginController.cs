﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MOD_AuthenticationService.Models;
using MOD_AuthenticationService.Repositories;

namespace MOD_AuthenticationService.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginRepository _repository;
        public LoginController(ILoginRepository repository)
        {
            _repository = repository;
        }
        
        [HttpGet]
        [Route("Validate/{email}/{password}")]
        public Token Get(string email, string password)
        {
            if (_repository.UserLogin(email, password) != null)
            {
                User response = _repository.UserLogin(email, password);
                return new Token() { message = "User", token = response.UserId.ToString() };
            }
            else if (_repository.MentorLogin(email, password)!= null)
            {
                Mentor response = _repository.MentorLogin(email, password);
                return new Token() { message = "Mentor", token = response.MentorId.ToString()};
            }
            else if (email == "Coursera" && password == "admin")
            {
                return new Token() { message = "Admin", token = "Admin" };
            }
            else
            {
                return new Token() { message = "Invalid User/Mentor", token = "" };
            }
        }
        public string GetToken()
        {
            var _config = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json").Build();
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var expiry = DateTime.Now.AddMinutes(120);
            var securityKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials
        (securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer: issuer,
        audience: audience,
        expires: DateTime.Now.AddMinutes(120),
        signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            var stringToken = tokenHandler.WriteToken(token);
            return stringToken;
        }



    }
}
