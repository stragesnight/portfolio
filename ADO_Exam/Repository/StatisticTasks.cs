using System;
using System.Linq;
using System.Collections.Generic;

namespace ADO_Exam.Repository
{
    public static class StatisticTasks
    {
        public static List<GameDbTask> GetTasks()
        {
            return new List<GameDbTask> {
                new GameDbTask {
                    TaskName = "Полная информация об играх",
                    Query = (_) => Controller.GetFormattedGameList(Controller.GetGames())
                },
                new GameDbTask {
                    TaskName = "Количество однопользовательских игр",
                    Query = (_) => new List<object> { new {
                        SinglePlayerCount = Controller.FindGames(x => x.GameMode.Name == "Однопользовательский").Count()
                    } }
                },
                new GameDbTask {
                    TaskName = "Количество многопользовательских игр",
                    Query = (_) =>  new List<object> { new {
                        MultiPlayerCount = Controller.FindGames(x => x.GameMode.Name == "Многопользовательский").Count()
                    } }
                },
                new GameDbTask {
                    TaskName = "Самая продаваемая игра конкретного стиля",
                    Query = (arg) => Controller.GetFormattedGameList(
                        Controller.FindGames(x => x.Genre.Name == arg).OrderByDescending(x => x.CopiesSold).Take(1)),
                    HasParameter = true
                },
                new GameDbTask {
                    TaskName = "Топ-5 самых продаваемых игр конкретного стиля",
                    Query = (arg) => Controller.GetFormattedGameList(
                        Controller.FindGames(x => x.Genre.Name == arg).OrderByDescending(x => x.CopiesSold).Take(5)),
                    HasParameter = true
                },
                new GameDbTask {
                    TaskName = "Топ-5 самых непродаваемых игр конкретного стиля",
                    Query = (arg) => Controller.GetFormattedGameList(
                        Controller.FindGames(x => x.Genre.Name == arg).OrderBy(x => x.CopiesSold).Take(5)),
                    HasParameter = true
                },
                new GameDbTask {
                    TaskName = "Полная информация об игре по названию",
                    Query = (arg) => Controller.GetFormattedGameList(Controller.FindGames(x => x.Title == arg)),
                    HasParameter = true
                },
                new GameDbTask {
                    TaskName = "Самый популярный стиль у каждой студии",
                    Query = (_) => Controller.GetCompanies().GroupBy(x => x.Name).Select(x => new {
                        CompanyName = x.Key,
                        MostPopularGenre = Controller.GetGames().Where(x1 => x1.Company.Name == x.Key)
                            .GroupBy(x1 => x1.Genre.Name).Select(x1 => new { GenreName = x1.Key, Count = x1.Count() })
                            .OrderByDescending(x1 => x1.Count).FirstOrDefault().GenreName
                    })
                }
            };
        }
    }
}
