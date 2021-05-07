﻿using DotNetAuthentication.DB;
using DotNetAuthentication.Models;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;
using JWT.Serializers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DotNetAuthentication.Controllers
{
    //have authorize here 
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly NBAContext _context;
        // NBA_DBContext context = new NBA_DBContext();


        public UserController(NBAContext context)
        {
            _context = context;

        }
       //Should be POST return should be Ok(...) use taskaction result
        [HttpPut("/register")]
        public async Task<bool> RegisterUser([FromBody] User user)
        {
            var userCheck = await _context.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName);

            if (userCheck == null)
            {
                user.PasswordHash = user.Hash(user.PasswordHash);
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        [HttpPost("/Login")]
        public ActionResult Login([FromBody] Login user)
        {//TODO create a login model
            //move check pass function to that model

            var isUser = _context.Users.FirstOrDefault(x => x.UserName == user.UserName);

            if (isUser == null) { return Ok("Incorrect credentials"); }

            var SavedPasswordHash = _context.Users.FirstOrDefault(x => x.UserName == user.UserName).PasswordHash;

            

            var isValid = user.CheckPass(SavedPasswordHash, user.PasswordHash);
            
            //Check if valid user
            if (isValid)
            {
                var UserId = _context.Users.FirstOrDefault(x => x.UserName == user.UserName).UserId;
                //Generate Token
                var token = new Authorise();
                var Token = token.Generate(UserId);

                //token needs to be stored in a http cookie client side
                //UserId needs to be extracted from token and database needs to filter based on the user            
                
                return Ok(Token);
            }

            return Ok("Invalid User name or password.");

                       
        }

        [HttpGet("/refreshtoken")]
        public string RefreshToken([FromHeader] string Token)
        {//lower case paramaters
            //See all teams the current user has.
            try
            {   
                //Validate Token
                var authorise = new Authorise();
                var userId = authorise.Validate(Token);

                var token = authorise.Generate(userId);

                //new token is sent to client            
                return token;

            }
            catch (TokenExpiredException)
            {
               throw new ArgumentException("Token has expired");
            }
            catch (SignatureVerificationException)
            {
                throw new ArgumentException("Token has invalid signature");
            }  
        }
    }
}

//TODO 
// do more research on bearer tokens 
// Look into Audience 
// Look into Issuer
