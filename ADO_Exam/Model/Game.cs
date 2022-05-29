using System;

namespace ADO_Exam.Model
{
    public class Game
    {
        public Game() { }

        public Game(int genreId, int companyId, int gameModeId, int copiesSold, DateTime releaseDate, string title)
        {
            GenreId = genreId;
            CompanyId = companyId;
            GameModeId = gameModeId;
            CopiesSold = copiesSold;
            ReleaseDate = releaseDate;
            Title = title;
        }

        public int Id { get; set; }
        public int GenreId { get; set; }
        public int CompanyId { get; set; }
        public int GameModeId { get; set; }
        public int CopiesSold { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Title { get; set; }

        public virtual Genre Genre { get; set; }
        public virtual Company Company { get; set; }
        public virtual GameMode GameMode { get; set; }
    }
}
