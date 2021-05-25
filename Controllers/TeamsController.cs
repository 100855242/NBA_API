using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetAuthentication.DB;
using DotNetAuthentication.Models;
using JWT.Exceptions;
using System.Diagnostics;
using System.Data;

namespace DotNetAuthentication.Controllers
{
    //add authorize [authorize]
    [ApiController]
    [Route("[controller]")]

    public class TeamsController : Controller
    {
        private readonly NBAContext _context;

        public TeamsController(NBAContext context)
        {
            _context = context;
        }

        //Add Team
        [HttpPost("addteam")]
        public async Task<ActionResult<bool>> PostTeam([FromBody] TeamUpdate input)
        {
            try
            {
                //Validate Token
                var authorise = new Authorise();
                var userId = authorise.Validate(input.Token);

                var isTeam = _context.Team
                    .Where(t => t.TeamName == input.TeamName)
                    .Where(u => u.UserId == userId)
                    .FirstOrDefault();

                //if Team already exists
                if (isTeam != null) { return Ok(false); }

                //Create team object
                var team = new Team();
                team.TeamName = input.TeamName;
                team.UserId = userId;
                team.isFav = false;
                team.PlayerCount = 0;

                //insert into database
                _context.Team.Add(team);
                await _context.SaveChangesAsync();

            }

            catch (TokenExpiredException)
            {
                throw new ArgumentException("Token has expired");
            }
            catch (SignatureVerificationException)
            {
                throw new ArgumentException("Token has invalid signature");
            }

            catch (DbUpdateException e)
            {
                Debug.WriteLine(e.Message);
                throw;

            }

            return Ok(true);//return ok Return OK(true)
        }

        //Delete team 
        [HttpDelete("deleteteam")]
        public  void  DeleteTeam([FromBody] TeamUpdate input)
        {
            //Delete Team
            try
            {                
                //Validate Token
                var authorise = new Authorise();
                var userId = authorise.Validate(input.Token);


                var team = new Team();
                team.UserId = userId;
                team.TeamName = input.TeamName;

                //check if Team exists 
                var isTeam = _context.Team
                    .Where(t => t.UserId == team.UserId)
                    .Where(t => t.TeamName == team.TeamName)
                    .FirstOrDefault().TeamName == team.TeamName;

                //If Team Exists
                if (isTeam)
                {
                    //delete team
                    _context.RemoveRange(
                        _context.Team
                        .Where(t => t.UserId == team.UserId)
                        .Where(t => t.TeamName == team.TeamName)
                        .FirstOrDefault());
                     _context.SaveChangesAsync();                                            
                }                
            }

            catch (TokenExpiredException)
            {
                throw new ArgumentException("Token has expired");
            }
            catch (SignatureVerificationException)
            {
                throw new ArgumentException("Token has invalid signature");
            }
            catch (DbUpdateException e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        [HttpPost("getteams")]
        public async Task<ActionResult<IEnumerable<DtrScores>>> GetTeams([FromBody] Token token)
        {
            try
            {
                // Validate Token
                var authorise = new Authorise();
                var userId = authorise.Validate(token.token);

                //Show Users teams
                var team = await _context.DtrScores.FromSqlRaw("DtrScores @p0", userId)
                    .ToListAsync();
                return Ok(team);

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

        [HttpPost("searchteams")]
        public async Task<ActionResult<IEnumerable<Team>>> SearchTeams([FromQuery] string filter, [FromBody] Token token)
        {
            try
            {
                // Validate Token
                var authorise = new Authorise();
                var userId = authorise.Validate(token.token);

                //Show Users teams
                var team = await _context.DtrScores
                    .FromSqlRaw("DtrScoresSearch @p0, @p1", userId, filter)
                    .ToListAsync();

                return Ok(team);

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

        //Update users current teams to favorite
        [HttpPut("setfavorites")]
        public void SetFavorites([FromBody] FavoriteTeam fav)
        {
            try
            {
                // Validate Token
                var authorise = new Authorise();
                var userId = authorise.Validate(fav.Token);

                
                    var teamUpdate =  _context.Team
                    .Where(t => t.TeamName == fav.TeamNames)
                    .Where(t => t.UserId == userId)
                    .FirstOrDefault();
                                        

                    teamUpdate.isFav = fav.IsFav;
                     _context.SaveChangesAsync();                                
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

        ////Get Favorite Teams 
        //[HttpPost("getfavorites")]
        //public async Task<ActionResult<IEnumerable<Team>>> GetFavorites([FromBody] string token)
        //{
        //    try
        //    {
        //        // Validate Token
        //        var authorise = new Authorise();
        //        var userId = authorise.Validate(token);

        //        //Show Users teams
        //        var output = await _context.DtrScores
        //            .FromSqlRaw("DtrScoresFav @p0", userId)
        //            .ToListAsync();

        //        return Ok(output);
        //    }

        //    catch (TokenExpiredException)
        //    {
        //        throw new ArgumentException("Token has expired");
        //    }
        //    catch (SignatureVerificationException)
        //    {
        //        throw new ArgumentException("Token has invalid signature");
        //    }

        //}
    }
}
