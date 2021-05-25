using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetAuthentication.DB;
using DotNetAuthentication.Models;
using JWT;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Microsoft.Data.SqlClient;

namespace DotNetAuthentication.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PlayersController : Controller
    {
        private readonly NBAContext _context;

        public PlayersController(NBAContext context)
        {
            _context = context;
        }

        // Get all players
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetallPlayers([FromQuery] PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.SortString, filter.SortOrder);
            var totalRecords = await _context.allPlayers.CountAsync();
            var pagesCount = (decimal)totalRecords / (decimal)filter.PageSize;
            var pagedData = await _context.allPlayers
            // .OrderByDescending(p =>"p."+validFilter.SortString)
            .OrderBy(p => EF.Property<object>(p, validFilter.SortString))
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToListAsync();
            if ((pagesCount % 1) != 0)
            {
                pagesCount = Decimal.ToInt32(pagesCount);
                pagesCount += 1;
            }
            if (filter.SortOrder == "ASC")
            {
                return Ok(new Response<List<Player>>(pagedData, Decimal.ToInt32(pagesCount)));

            }
            else if (filter.SortOrder == "DESC")
            {
                var pagedData1 = await _context.allPlayers
           // .OrderByDescending(p =>"p."+validFilter.SortString)
           .OrderByDescending(p => EF.Property<object>(p, validFilter.SortString))
           .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
           .Take(validFilter.PageSize)
           .ToListAsync();
                return Ok(new Response<List<Player>>(pagedData1, Decimal.ToInt32(pagesCount)));
            }

            return Ok(new Response<List<Player>>(pagedData, Decimal.ToInt32(pagesCount)));
        }

        [HttpGet("SearchPlayer")]
        //this code could be better
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayer([FromQuery] SearchPaginationFilter filter)
        {

            // partial first name and partial lastname search or player initials with pagination
            var validFilter = new SearchPaginationFilter(filter.searchstring, filter.PageNumber, filter.PageSize, filter.SortString, filter.SortOrder);

            string[]? splitString = filter.searchstring?.Split(' ');
            var pagedData = new List<Player>();
            decimal pagescount = 0;


            if (splitString?.Length == 2)
            {
                //filters the player view data based on the searchstring and adds pagination based on the url.
                var pageData = await _context.allPlayers.Where(p => EF.Functions.Like(p.FIRSTNAME, $"{splitString[0]}%") && EF.Functions.Like(p.LASTNAME, $"{splitString[1]}%"))
                    .OrderBy(p => EF.Property<object>(p, validFilter.SortString))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();

                //total pages in current search
                var totalRecords = await _context.allPlayers.Where(p => EF.Functions.Like(p.FIRSTNAME, $"{splitString[0]}%") && EF.Functions.Like(p.LASTNAME, $"{splitString[1]}%"))
                    .OrderBy(p => p.FIRSTNAME).CountAsync();

                var pagesCount = (decimal)totalRecords / (decimal)filter.PageSize;

                if ((pagesCount % 1) != 0)
                {
                    pagesCount = Decimal.ToInt32(pagesCount);
                    pagesCount += 1;
                }
                if (filter.SortOrder == "ASC")
                {
                    return Ok(new Response<List<Player>>(pageData, Decimal.ToInt32(pagesCount)));

                }
                else if (filter.SortOrder == "DESC")
                {
                    pageData = await _context.allPlayers.Where(p => EF.Functions.Like(p.FIRSTNAME, $"{splitString[0]}%") && EF.Functions.Like(p.LASTNAME, $"{splitString[1]}%"))
                       .OrderByDescending(p => EF.Property<object>(p, validFilter.SortString))
                       .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                       .Take(validFilter.PageSize)
                       .ToListAsync();

                    return Ok(new Response<List<Player>>(pageData, Decimal.ToInt32(pagesCount)));
                }
            }
            else
            {
                //Partial firstname + lastname search this code runs if the splitString array has more then 2 items
                var pageData = await _context.allPlayers
                    .Where(p => EF.Functions.Like(p.FIRSTNAME, $"{filter.searchstring}%") || EF.Functions.Like(p.LASTNAME, $"{filter.searchstring}%") || EF.Functions.Like(p.FIRSTNAME + " " + p.LASTNAME, $"{filter.searchstring}%"))
                      .OrderBy(p => EF.Property<object>(p, validFilter.SortString))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();

                //total pages in current search
                var totalRecords = await _context.allPlayers.Where(p => EF.Functions.Like(p.FIRSTNAME, $"{filter.searchstring}%") || EF.Functions.Like(p.LASTNAME, $"{filter.searchstring}%") || EF.Functions.Like(p.FIRSTNAME + " " + p.LASTNAME, $"{filter.searchstring}%")).CountAsync();

                var pagesCount = (decimal)totalRecords / (decimal)filter.PageSize;

                if ((pagesCount % 1) != 0)
                {
                    pagesCount = Decimal.ToInt32(pagesCount);
                    pagesCount += 1;
                }
                if (filter.SortOrder == "ASC")

                {
                    pagedData = pageData;
                    pagescount = pagesCount;
                    return Ok(new Response<List<Player>>(pageData, Decimal.ToInt32(pagesCount)));

                }
                else if (filter.SortOrder == "DESC")
                {
                    pageData = await _context.allPlayers
                    .Where(p => EF.Functions.Like(p.FIRSTNAME, $"{filter.searchstring}%") || EF.Functions.Like(p.LASTNAME, $"{filter.searchstring}%") || EF.Functions.Like(p.FIRSTNAME + " " + p.LASTNAME, $"{filter.searchstring}%"))
                      .OrderByDescending(p => EF.Property<object>(p, validFilter.SortString))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
                    return Ok(new Response<List<Player>>(pageData, Decimal.ToInt32(pagesCount)));
                }

            }
            return Ok(new Response<List<Player>>(pagedData, Decimal.ToInt32(pagescount)));
        }

        //Get Column Headers
        [HttpGet("headers")]

        public async Task<ActionResult<IEnumerable<ColumnHeaders>>> GetHeaders()
        {
            var Data = await _context.columnHeaders
                .ToListAsync();

            //return Ok(new Response<List<ColumnHeaders>>(Data));
            return Ok(Data);
        }

        // GET: api/Player/5
        // search player by player_key
        [HttpGet("Player_key")]
        public async Task<ActionResult<Player>> GetPlayer(int Player_key)
        {
            var player = await _context.allPlayers.FindAsync(Player_key);

            if (player == null)
            {
                return NotFound();
            }

            return player;
        }

        // Get players from team for a user
        [Route("getPlayersFromTeam")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Player>>> getPlayersFromTeam([FromBody] FullTeamRosterRequest teamReq)
        {
            //See all teams the current user has.
            try
            { 
                //Validate Token
                var authorise = new Authorise();
                var userId = authorise.Validate(teamReq.Token);

                //If valid send requested data
                var userInput = teamReq.TeamName;
                var usortcol = teamReq.SortString;
                var uSortType = teamReq.SortType;

                var pagedData = await _context.allPlayers.FromSqlRaw
                    ("getPlayersFromTeam @p0,@p1,@p2,@p3", userId, userInput, usortcol, uSortType).ToListAsync();

                //SqlCommand cmd = new SqlCommand("DtrScore");
                //cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@teamName", userInput);
                //cmd.Parameters.AddWithValue("@userId", userId);

                var parameterReturn = new SqlParameter
                {
                    ParameterName = "dtr",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output,
                };

                var dtrScore = _context.Database.ExecuteSqlRaw("EXEC @dtr = [dbo].[DtrScore] @p0, @p1", userId, userInput, parameterReturn);

                int dtr = (int)parameterReturn.Value;             

                var result = new { pagedData, dtr };

                return Ok(result);
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
