using System;
using System.Drawing;
using System.Windows.Forms;
using ADO_Exam.Model;
using ADO_Exam.Repository;

namespace ADO_Exam.View
{
    public partial class AddUpdateInfoForm : Form
    {
        public enum WorkMode
        {
            AddNewEntry,
            UpdateExistingEntry
        };

        private WorkMode _workMode = WorkMode.AddNewEntry;
        private Game _foundGame = null;

        public AddUpdateInfoForm(WorkMode workMode)
        {
            InitializeComponent();
            _workMode = workMode;
        }

        private bool IsStateValid()
        {
            bool valid = true;
            valid &= !String.IsNullOrEmpty(textBox3.Text);
            valid &= !String.IsNullOrEmpty(textBox4.Text);
            valid &= !String.IsNullOrEmpty(textBox5.Text);
            valid &= !String.IsNullOrEmpty(textBox6.Text);
            valid &= uint.TryParse(textBox7.Text, out uint _);

            if (!valid)
                MessageBox.Show("Введены некорректные данные");

            return valid;
        }

        private void HandleAddGame()
        {
            if (!IsStateValid())
                return;

            Game toAdd = new Game {
                Title = textBox3.Text,
                GenreId = Controller.FindOrAddGenre(textBox4.Text).Id,
                GameModeId = Controller.FindOrAddGameMode(textBox5.Text).Id,
                CompanyId = Controller.FindOrAddCompany(textBox6.Text).Id,
                CopiesSold = int.Parse(textBox7.Text),
                ReleaseDate = dateTimePicker1.Value.Date
            };

            if (Controller.AddGame(toAdd))
                MessageBox.Show("Игра была успешно добавлена");
            else
                MessageBox.Show("Возникла ошибка при добавлении новой игры");
        }

        private void HandleFindGame()
        {
            Game game = Controller.FindGame(x => x.Title == textBox1.Text && x.Company.Name == textBox2.Text);
            if (game == null)
            {
                MessageBox.Show("Игра не найдена");
                return;
            }

            groupBox2.Enabled = true;
            textBox3.Text = game.Title;
            textBox4.Text = game.Genre.Name;
            textBox5.Text = game.GameMode.Name;
            textBox6.Text = game.Company.Name;
            textBox7.Text = game.CopiesSold.ToString();
            dateTimePicker1.Value = game.ReleaseDate;

            _foundGame = game;
        }

        private void HandleUpdateGame()
        {
            if (!IsStateValid())
                return;

            Game updated = new Game {
                Title = textBox3.Text,
                GenreId = Controller.FindOrAddGenre(textBox4.Text).Id,
                GameModeId = Controller.FindOrAddGameMode(textBox5.Text).Id,
                CompanyId = Controller.FindOrAddCompany(textBox6.Text).Id,
                CopiesSold = int.Parse(textBox7.Text),
                ReleaseDate = dateTimePicker1.Value.Date
            };

            if (Controller.UpdateGame(_foundGame, updated))
                MessageBox.Show("Игра успешно обновлена");
            else
                MessageBox.Show("Возникла ошибка при обновлении игры");
        }

        private void AddUpdateInfoForm_Load(object sender, EventArgs e)
        {
            switch (_workMode)
            {
                case WorkMode.AddNewEntry:
                    this.Text = "Добавление данных, " + this.Text;
                    groupBox2.Text = "Добавление " + groupBox2.Text;
                    groupBox1.Visible = false;
                    groupBox2.Location = new Point(97, 6);
                    button2.Text = "Добавить " + button2.Text;

                    button2.Click += (s, ea) => HandleAddGame();
                    break;
                case WorkMode.UpdateExistingEntry:
                    this.Text = "Изменение данных, " + this.Text;
                    groupBox2.Text = "Изменение " + groupBox2.Text;
                    button2.Text = "Изменить " + button2.Text;
                    groupBox2.Enabled = false;

                    button1.Click += (s, ea) => HandleFindGame();
                    button2.Click += (s, ea) => HandleUpdateGame();
                    break;
                default:
                    break;
            }
        }
    }
}
