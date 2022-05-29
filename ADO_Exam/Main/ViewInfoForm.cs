using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using ADO_Exam.Model;
using ADO_Exam.Repository;
using System.Threading.Tasks;

namespace ADO_Exam.View
{
    public partial class ViewInfoForm : Form
    {
        internal class SearchFilter
        {
            public string Title { get; set; } = String.Empty;
            public string Genre { get; set; } = String.Empty;
            public string GameMode { get; set; } = String.Empty;
            public string Company { get; set; } = String.Empty;
            public int CopiesSoldFrom { get; set; } = 0;
            public int CopiesSoldTo { get; set; } = 0;
            public DateTime ReleaseDateFrom { get; set; } = DateTime.Today;
            public DateTime ReleaseDateTo { get; set; } = DateTime.Today;
            public SortMode SortMode { get; set; } = SortMode.Title;
            public bool SortByDescending { get; set; } = true;
            public int TakeNum { get; set; } = -1;
        }

        internal enum SortMode
        {
            Title = 0,
            Genre,
            GameMode,
            Company,
            CopiesSold,
            ReleaseDate
        };

        public ViewInfoForm()
        {
            InitializeComponent();
        }

        private SearchFilter CompileSearchFilter()
        {
            if (this.InvokeRequired)
                return (SearchFilter)this.Invoke(new Func<SearchFilter>(CompileSearchFilter));

            if (!int.TryParse(textBox2.Text, out int copiesFrom))
                copiesFrom = 0;
            if (!int.TryParse(textBox3.Text, out int copiesTo))
                copiesTo = 0;
            if (!int.TryParse(textBox5.Text, out int take))
                take = 0;

            return new SearchFilter {
                Title = textBox1.Text,
                Genre = comboBox1.SelectedItem.ToString(),
                GameMode = comboBox2.SelectedItem.ToString(),
                Company = textBox4.Text,
                CopiesSoldFrom = copiesFrom,
                CopiesSoldTo = copiesTo,
                ReleaseDateFrom = dateTimePicker1.Value.Date,
                ReleaseDateTo = dateTimePicker2.Value.Date,
                SortMode = (SortMode)comboBox3.SelectedIndex,
                SortByDescending = radioButton1.Checked,
                TakeNum = take
            };
        }

        private Task<IEnumerable<Game>> GetGamesFilteredAsync(SearchFilter filter)
        {
            return Task.Run(() => {
                IEnumerable<Game> games = Controller.GetGames();

                if (!String.IsNullOrEmpty(filter.Title))
                    games = games.Where(x => x.Title == filter.Title);
                if (!String.IsNullOrEmpty(filter.Genre))
                    games = games.Where(x => x.Genre.Name == filter.Genre);
                if (!String.IsNullOrEmpty(filter.GameMode))
                    games = games.Where(x => x.GameMode.Name == filter.GameMode);
                if (!String.IsNullOrEmpty(textBox4.Text))
                    games = games.Where(x => x.Company.Name == textBox4.Text);
                if (filter.CopiesSoldFrom > 0)
                    games = games.Where(x => x.CopiesSold >= filter.CopiesSoldFrom);
                if (filter.CopiesSoldTo > 0)
                    games = games.Where(x => x.CopiesSold <= filter.CopiesSoldTo);
                if (filter.ReleaseDateFrom <= DateTime.Today)
                    games = games.Where(x => x.ReleaseDate >= filter.ReleaseDateFrom);
                if (filter.ReleaseDateTo <= DateTime.Today)
                    games = games.Where(x => x.ReleaseDate <= filter.ReleaseDateTo);

                switch (filter.SortMode)
                {
                    case SortMode.Title:
                        games = games.OrderBy(x => x.Title);
                        break;
                    case SortMode.Genre:
                        games = games.OrderBy(x => x.Genre.Name);
                        break;
                    case SortMode.GameMode:
                        games = games.OrderBy(x => x.GameMode.Name);
                        break;
                    case SortMode.Company:
                        games = games.OrderBy(x => x.Company.Name);
                        break;
                    case SortMode.CopiesSold:
                        games = games.OrderBy(x => x.CopiesSold);
                        break;
                    case SortMode.ReleaseDate:
                        games = games.OrderBy(x => x.ReleaseDate);
                        break;
                    default:
                        break;
                }

                if (filter.SortByDescending)
                    games = games.Reverse();
                if (filter.TakeNum > 0)
                    games = games.Take(filter.TakeNum);

                return games;
            });
        }

        private void UpdateDataGridView(IEnumerable<object> dataSource)
        {
            if (dataGridView2.InvokeRequired)
            {
                dataGridView2.Invoke(new Action<IEnumerable<object>>(UpdateDataGridView), dataSource);
                return;
            }

            dataGridView2.DataSource = dataSource.ToList();
        }

        private void ResizeDataGridViewCollumns()
        {
            if (dataGridView2.InvokeRequired)
            {
                dataGridView2.Invoke(new Action(ResizeDataGridViewCollumns));
                return;
            }

            for (int i = 0; i < dataGridView2.Columns.Count; ++i)
                dataGridView2.Columns[i].Width = dataGridView2.Columns[i]
                    .GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, true);
        }

        private void ViewInfoForm_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add(String.Empty);
            comboBox1.Items.AddRange(Controller.GetGenres().Select(x => x.Name).ToArray());
            comboBox1.SelectedIndex = 0;

            comboBox2.Items.Clear();
            comboBox2.Items.Add(String.Empty);
            comboBox2.Items.AddRange(Controller.GetGameModes().Select(x => x.Name).ToArray());
            comboBox2.SelectedIndex = 0;

            comboBox3.Items.Clear();
            comboBox3.Items.AddRange(new string[] {
                "По названию",
                "По жанру",
                "По режиму игры",
                "По создателю",
                "По кол-ву копий",
                "По дате релиза"
            });
            comboBox3.SelectedIndex = 0;

            dateTimePicker1.Value = new DateTime(1950, 1, 1);
            dateTimePicker2.Value = DateTime.Today;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            UpdateDataGridView(new List<object> { new { Hint = "Поиск..." } });
            IEnumerable<Game> games = await GetGamesFilteredAsync(CompileSearchFilter());
            UpdateDataGridView(Controller.GetFormattedGameList(games));
            ResizeDataGridViewCollumns();
        }
    }
}
