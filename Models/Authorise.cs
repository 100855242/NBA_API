using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;
using JWT.Serializers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;

namespace DotNetAuthentication.Models
{
    class Authorise
    {
        const string secret = "266A96DB-AE6D-40CD-87Fh1-F845FB4C44EC5E89B0AE-8C36-487F-AB74-147F701C6588A5258ABA-25E2-40AD-B5A2-26F0C1D99883";

        public Token Generate(int UserId)
        {   
            
            //should return as JASON
            //issue Token            
            var token = JwtBuilder.Create()
                  .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                  .WithSecret(secret)
                  .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds())
                  .AddClaim("User", $"{UserId}")
                  .MustVerifySignature()
                  .Encode();

            Token thisToken = new Token();
            thisToken.token = token;            

            return thisToken;
        }

        public int Validate(string Token)
        {
            
            try
            {   
                //Validate token
                IJsonSerializer serializer = new JsonNetSerializer();
                var provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // symmetric
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);

                var json = decoder.Decode(Token, secret, verify: true);
                dynamic data = JObject.Parse(json);

                //extract user Id
                var userId = (int)data.User;
                return userId;

                // var validFilter = new FullTeamRosterRequest(teamReq.PageNumber, teamReq.PageSize, teamReq.SortString, teamReq.TeamName);    
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