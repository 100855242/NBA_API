using DotNetAuthentication.DB;
using DotNetAuthentication.Models;
using JWT.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetAuthentication.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PlayerSelectionController : ControllerBase
    {
        private readonly NBAContext _context;

        public PlayerSelectionController(NBAContext context)
        {
            _context = context;
        }

        // GET: api/PlayerSelection
        /// display all Players and teams
        [HttpPut]
        public async Task<ActionResult<IEnumerable<PlayerSelection>>> GetPlayerSelection([FromBody] Token token)
        {
            try
            {
                //Validate Token
                var authorise = new Authorise();
                var userId = authorise.Validate(token.token);

                return await _context.PlayerSelection
                .Where(p => p.UserId == userId)
                .ToListAsync();
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

        ///Update Player Selection
        [HttpPost("UpdatePlayerSelection")]
        public void UpdatePlayers([FromBody] PlayerSelections selections)
        {
            try
            {                
                //Validate Token
                var authorise = new Authorise();
                var userId = authorise.Validate(selections.Token);

                //TODO check if team exists

                //count players currently in team
                var PlayerCount = _context.PlayerSelection
                    .Where(p => p.TeamName == selections.TeamName)
                    .Where(u => u.UserId == userId)
                    .CountAsync()
                    .Result;

                var players = _context.PlayerSelection
                    .Where(p => p.TeamName == selections.TeamName)
                    .Where(p => p.UserId == userId)
                    .ToList();

                //remove players in current team
                foreach(var player in players)
                {
                    _context.PlayerSelection.Remove(player);
                    _context.SaveChanges();
                }
                
                //save changes to DB
                _context.SaveChanges();

                //update playerCount
                PlayerCount = 0;
                
                //add players to player selection table
                foreach (int i in selections.PlayerKeys)
                {
                    //Limit Players on a team to 15
                    if(PlayerCount < 15)
                    { 
                        var selection = new PlayerSelection(selections.TeamName, i, userId);
                        _context.PlayerSelection.Add(selection);

                         _context.SaveChanges();

                        PlayerCount += 1;
                    }
                    else
                    {
                        break;
                    }
                }

                // select current team                   
                var team = _context.Team
                    .Where(t => t.TeamName == selections.TeamName)
                    .Where(t => t.UserId == userId)
                    .First();

                    //update team player count
                    team.PlayerCount = PlayerCount;
                     _context.SaveChangesAsync();                    
                }                            

            catch (DbUpdateException)
            {
                throw new ArgumentException("Unique player list must be selected");
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

        // GET: api/PlayerSelection/5
        // display players from searching team name
        // [HttpGet("ViewPlayers")]
        // public async Task<ActionResult<PlayerSelection>> GetPlayerSelection([FromQuery] string searchstring)
        // {
        //     var DisplayData = await _context.PlayerSelection.Where(p => EF.Functions.Like(p.TeamName, $"{searchstring}%"))
        //            .OrderBy(p => p.TeamName)
        //            .ToListAsync();

        //     return Ok(new Response<List<PlayerSelection>>(DisplayData)); ;
        // }

        ////Add Players to team
        //[HttpPost("addplayer")]
        //public async Task<bool> PostPlayer([FromBody] PlayerSelections selections)
        //{
        //    try
        //    {
        //        //check if team exists
        //        //Validate Token
        //        var authorise = new Authorise();
        //        var userId = authorise.Validate(selections.Token);

        //        selections.UserId = userId;

        //        //count players currently in team
        //        var PlayerCount = _context.PlayerSelection
        //            .Where(p => p.TeamName == selections.TeamName)
        //            .Where(u => u.UserId == userId)
        //            .CountAsync()
        //            .Result;

        //        var totalPlayers = PlayerCount + selections.PlayerKeys.Length;

        //        //Limit players on a team to 15
        //        if (totalPlayers <= 15)
        //        {

        //            //add players to player selection table
        //            foreach (int i in selections.PlayerKeys)
        //            {
        //                var selection = new PlayerSelection(selections.TeamName, i, userId);
        //                _context.PlayerSelection.Add(selection);

        //                await _context.SaveChangesAsync();
        //            }

        //            // select current team                   
        //            var team = await _context.Team
        //                .Where(t => t.TeamName == selections.TeamName)
        //                .Where(t => t.UserId == userId)
        //                .FirstAsync();

        //            //update team player count
        //            team.PlayerCount = totalPlayers;
        //            await _context.SaveChangesAsync();

        //            return true;
        //        }

        //        return false;
        //    }

        //    catch (DbUpdateException)
        //    {
        //        throw new ArgumentException("Unique player list must be selected");
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


        // DELETE: api/PlayerSelection/DeletePlayer
        // [HttpDelete("DeletePlayer")]
        // public async Task<bool> DeletePlayerSelection([FromBody] PlayerSelection playerSelection)
        // {
        //     var PlayerDeleted = _context.PlayerSelection.Where(p => p.TeamName == playerSelection.TeamName);

        //     try
        //     {
        //         _context.PlayerSelection.Remove(playerSelection);
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateException)
        //     {
        //         return false;
        //     }

        //     return true;
        // }


    }
}
