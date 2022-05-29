using System;
using System.Data.Entity;
using ADO_Exam.Model;

namespace ADO_Exam.Repository
{
    public class GameDbContext : DbContext
    {
        public GameDbContext() : base("GameDB")
        {
            Database.SetInitializer(new GameDbInitializer());
        }

        public DbSet<City> Cities { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyBranch> CompanyBranches { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<GameMode> GameModes { get; set; }
    }
}
