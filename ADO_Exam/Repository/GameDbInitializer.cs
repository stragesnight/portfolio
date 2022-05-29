using System;
using System.Linq;
using System.Data.Entity;
using ADO_Exam.Model;

namespace ADO_Exam.Repository
{
    class GameDbInitializer : DropCreateDatabaseAlways<GameDbContext>
    {
        private static readonly int _nEntries = 20;
        private GameDbContext _dbContext = null;

        private void AddCities()
        {
            int countryCount = _dbContext.Countries.Count();
            for (int i = 1; i <= _nEntries; ++i)
                _dbContext.Cities.Add(new City {
                    CountryId = i % countryCount + 1,
                    Name = $"City{i}"
                });
            _dbContext.SaveChanges();
        }

        private void AddCompanies()
        {
            for (int i = 1; i <= _nEntries / 3; ++i)
                _dbContext.Companies.Add(new Company { Name = $"Company{i}" });
            _dbContext.SaveChanges();
        }

        private void AddCompanyBranches()
        {
            int companyCount = _dbContext.Companies.Count();
            int cityCount = _dbContext.Cities.Count();
            for (int i = 1; i <= _nEntries; ++i)
                _dbContext.CompanyBranches.Add(new CompanyBranch {
                    CompanyId = i % companyCount + 1,
                    CityId = i % cityCount + 1
                });
            _dbContext.SaveChanges();
        }

        private void AddCountries()
        {
            for (int i = 1; i <= _nEntries / 4; ++i)
                _dbContext.Countries.Add(new Country { Name = $"Country{i}" });
            _dbContext.SaveChanges();
        }

        private void AddGames()
        {
            int companyCount = _dbContext.Companies.Count();
            int genreCount = _dbContext.Genres.Count();
            int gameModeCount = _dbContext.GameModes.Count();
            for (int i = 1; i <= _nEntries; ++i)
                _dbContext.Games.Add(new Game {
                    CompanyId = i % companyCount + 1,
                    GenreId = i % genreCount + 1,
                    GameModeId = i  % gameModeCount + 1,
                    CopiesSold = i * 10000 + i,
                    ReleaseDate = DateTime.Today,
                    Title = $"Game{i}"
                });
            _dbContext.SaveChanges();
        }

        private void AddGameModes()
        {
            _dbContext.GameModes.Add(new GameMode { Name = "Однопользовательский" });
            _dbContext.GameModes.Add(new GameMode { Name = "Многопользовательский" });
            _dbContext.SaveChanges();
        }

        private void AddGenres()
        {
            for (int i = 1; i <= _nEntries / 3; ++i)
                _dbContext.Genres.Add(new Genre { Name = $"Genre{i}" });
            _dbContext.SaveChanges();
        }

        protected override void Seed(GameDbContext context)
        {
            _dbContext = context;

            AddCountries();
            AddCities();
            AddCompanies();
            AddCompanyBranches();
            AddGameModes();
            AddGenres();
            AddGames();

            base.Seed(context);
        }
    }
}
