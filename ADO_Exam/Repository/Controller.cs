using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using ADO_Exam.Model;

namespace ADO_Exam.Repository
{
    public static class Controller
    {
        public static GameDbContext _dbContext = new GameDbContext();

        public static IEnumerable<City> GetCities() => _dbContext.Cities;
        public static City FindCity(Func<City, bool> predicate) => _dbContext.Cities.Where(predicate).FirstOrDefault();
        public static IEnumerable<Company> GetCompanies() => _dbContext.Companies;
        public static Company FindCompany(Func<Company, bool> predicate) => _dbContext.Companies.Where(predicate).FirstOrDefault();
        public static IEnumerable<CompanyBranch> GetCompanyBranches() => _dbContext.CompanyBranches;
        public static CompanyBranch FindCompanyBranch(Func<CompanyBranch, bool> predicate) => _dbContext.CompanyBranches.Where(predicate).FirstOrDefault();
        public static IEnumerable<Country> GetCountries() => _dbContext.Countries;
        public static Country FindCountry(Func<Country, bool> predicate) => _dbContext.Countries.Where(predicate).FirstOrDefault();
        public static IEnumerable<GameMode> GetGameModes() => _dbContext.GameModes;
        public static GameMode FindGameMode(Func<GameMode, bool> predicate) => _dbContext.GameModes.Where(predicate).FirstOrDefault();
        public static IEnumerable<Genre> GetGenres() => _dbContext.Genres;
        public static Genre FindGenre(Func<Genre, bool> predicate) => _dbContext.Genres.Where(predicate).FirstOrDefault();

        public static IEnumerable<Game> GetGames() => _dbContext.Games;
        public static Game FindGame(Func<Game, bool> predicate) => _dbContext.Games.Where(predicate).FirstOrDefault();
        public static IEnumerable<Game> FindGames(Func<Game, bool> predicate) => _dbContext.Games.Where(predicate);
        public static IEnumerable<object> GetFormattedGameList(IEnumerable<Game> games)
        {
            return games.Select(x => new {
                x.Title,
                Genre = x.Genre.Name,
                GameMode = x.GameMode.Name,
                Creator = x.Company.Name,
                CopiesSold = x.CopiesSold,
                ReleaseDate = x.ReleaseDate
            }).ToList<object>();
        }

        public static bool AddCity(City city)
        {
            try
            {
                _dbContext.Cities.Add(city);
                _dbContext.SaveChanges();
            }
            catch (Exception) { return false; }

            return true;
        }

        public static bool AddCompany(Company company)
        {
            try
            {
                _dbContext.Companies.Add(company);
                _dbContext.SaveChanges();
            }
            catch (Exception) { return false; }

            return true;
        }

        public static Company FindOrAddCompany(string name)
        {
            Company company = Controller.FindCompany(x => x.Name == name);
            if (company == null)
            {
                MessageBox.Show($"Компанию \"{name}\" не найдено. Она будет добавлена в базу данных.");
                Controller.AddCompany(new Company(name));
                company = Controller.FindCompany(x => x.Name == name);
            }
            return company;
        }

        public static bool AddCompanyBranch(CompanyBranch companyBranch)
        {
            try
            {
                _dbContext.CompanyBranches.Add(companyBranch);
                _dbContext.SaveChanges();
            }
            catch (Exception) { return false; }

            return true;
        }

        public static bool AddCountry(Country country)
        {
            try
            {
                _dbContext.Countries.Add(country);
                _dbContext.SaveChanges();
            }
            catch (Exception) { return false; }

            return true;
        }

        public static bool AddGame(Game game)
        {
            try
            {
                _dbContext.Games.Add(game);
                _dbContext.SaveChanges();
            }
            catch (Exception) { return false; }

            return true;
        }

        public static bool AddGameMode(GameMode gameMode)
        {
            try
            {
                _dbContext.GameModes.Add(gameMode);
                _dbContext.SaveChanges();
            }
            catch (Exception) { return false; }

            return true;
        }

        public static GameMode FindOrAddGameMode(string name)
        {
            GameMode gameMode = Controller.FindGameMode(x => x.Name == name);
            if (gameMode == null)
            {
                MessageBox.Show($"Режим игры \"{name}\" не найдено. Он будет добавлен в базу данных.");
                Controller.AddGameMode(new GameMode(name));
                gameMode = Controller.FindGameMode(x => x.Name == name);
            }
            return gameMode;
        }

        public static bool AddGenre(Genre genre)
        {
            try
            {
                _dbContext.Genres.Add(genre);
                _dbContext.SaveChanges();
            }
            catch (Exception) { return false; }

            return true;
        }

        public static Genre FindOrAddGenre(string name)
        {
            Genre genre = Controller.FindGenre(x => x.Name == name);
            if (genre == null)
            {
                MessageBox.Show($"Жанра \"{name}\" не найдено. Он будет добавлен в базу данных.");
                Controller.AddGenre(new Genre(name));
                genre = Controller.FindGenre(x => x.Name == name);
            }
            return genre;
        }

        public static bool RemoveCity(City city)
        {
            try
            {
                _dbContext.Cities.Remove(city);
                _dbContext.SaveChanges();
            }
            catch (Exception) { return false; }

            return true;
        }

        public static bool RemoveCompany(Company company)
        {
            try
            {
                _dbContext.Companies.Remove(company);
                _dbContext.SaveChanges();
            }
            catch (Exception) { return false; }

            return true;
        }

        public static bool RemoveCompanyBranch(CompanyBranch companyBranch)
        {
            try
            {
                _dbContext.CompanyBranches.Remove(companyBranch);
                _dbContext.SaveChanges();
            }
            catch (Exception) { return false; }

            return true;
        }

        public static bool RemoveCountry(Country country)
        {
            try
            {
                _dbContext.Countries.Remove(country);
                _dbContext.SaveChanges();
            }
            catch (Exception) { return false; }
            return true;
        }

        public static bool RemoveGame(Game game)
        {
            try
            {
                _dbContext.Games.Remove(game);
                _dbContext.SaveChanges();
            }
            catch (Exception) { return false; }
            return true;
        }

        public static bool RemoveGameMode(GameMode gameMode)
        {
            try
            {
                _dbContext.GameModes.Remove(gameMode);
                _dbContext.SaveChanges();
            }
            catch (Exception) { return false; }
            return true;
        }

        public static bool RemoveGenre(Genre genre)
        {
            try
            {
                _dbContext.Genres.Remove(genre);
                _dbContext.SaveChanges();
            }
            catch (Exception) { return false; }
            return true;
        }

        public static bool UpdateCity(City city, City toBe)
        {
            try
            {
                city.Name = toBe.Name;
                _dbContext.SaveChanges();
            }
            catch (Exception) { return false; }
            return true;
        }

        public static bool UpdateCompany(Company company, Company toBe)
        {
            try
            {
                company.Name = toBe.Name;
                _dbContext.SaveChanges();
            }
            catch (Exception) { return false; }
            return true;
        }

        public static bool UpdateCompanyBranch(CompanyBranch companyBranch, CompanyBranch toBe)
        {
            try
            {
                companyBranch.CompanyId = toBe.CompanyId;
                companyBranch.CityId = toBe.CityId;
                _dbContext.SaveChanges();
            }
            catch (Exception) { return false; }
            return true;
        }

        public static bool UpdateCountry(Country country, Country toBe)
        {
            try
            {
                country.Name = toBe.Name;
                _dbContext.SaveChanges();
            }
            catch (Exception) { return false; }
            return true;
        }

        public static bool UpdateGame(Game game, Game toBe)
        {
            try
            {
                game.GenreId = toBe.GenreId;
                game.GameModeId = toBe.GameModeId;
                game.CompanyId = toBe.CompanyId;
                game.CopiesSold = toBe.CopiesSold;
                game.ReleaseDate = toBe.ReleaseDate;
                game.Title = toBe.Title;
                _dbContext.SaveChanges();
            }
            catch (Exception) { return false; }
            return true;
        }

        public static bool UpdateGameMode(GameMode gameMode, GameMode toBe)
        {
            try
            {
                gameMode.Name = toBe.Name;
                _dbContext.SaveChanges();
            }
            catch (Exception) { return false; }
            return true;
        }

        public static bool UpdateGenre(Genre genre, Genre toBe)
        {
            try
            {
                genre.Name = toBe.Name;
                _dbContext.SaveChanges();
            }
            catch (Exception) { return false; }
            return true;
        }
    }
}
