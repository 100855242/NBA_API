using DotNetAuthentication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetAuthentication.DB
{
    public class NBAContext : DbContext //:identity db context 
    {

        //entities
        public DbSet<User> Users { get; set; }
        public DbSet<Player> allPlayers { get; set; }        

        public DbSet<Team> Team { get; set; }

        public DbSet<PlayerSelection> PlayerSelection { get; set; }

        public DbSet<ColumnHeaders> columnHeaders { get; set; }

        public DbSet<DtrScores> DtrScores { get; set; }


        public NBAContext(DbContextOptions<NBAContext> options): base(options)
        {

        }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => new {
                u.UserId
            });

            modelBuilder.Entity<Player>().HasKey(p => new {
                p.Player_key
            });
            modelBuilder.Entity<Team>().HasKey(t => new
            {
                t.TeamName
            });

            modelBuilder.Entity<PlayerSelection>().HasKey(p => new
            {
                p.TeamName,
                p.Player_key,
                p.UserId
            });

            modelBuilder.Entity<ColumnHeaders>().HasNoKey();

            modelBuilder.Entity<DtrScores>().HasNoKey();
        }
    }
}
