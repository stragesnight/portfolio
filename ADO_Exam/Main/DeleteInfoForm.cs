using ADO_Exam.Model;
using ADO_Exam.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ADO_Exam.View
{
    public partial class DeleteInfoForm : Form
    {
        public DeleteInfoForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Game toDelete = Controller.FindGame(x => x.Title == textBox1.Text && x.Company.Name == textBox2.Text);
            if (toDelete == null)
            {
                MessageBox.Show("Игра не найдена");
                return;
            }

            if (MessageBox.Show($"Вы точно хотите удалить игру \"{toDelete.Title}\"?",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            if (Controller.RemoveGame(toDelete))
                MessageBox.Show("Игра успешно удалена");
            else
                MessageBox.Show("Возникла ошибка при удалении игры");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Company toDelete = Controller.FindCompany(x => x.Name == textBox3.Text);
            if (toDelete == null)
            {
                MessageBox.Show("Компания не найдена");
                return;
            }

            if (MessageBox.Show($"Вы точно хотите удалить компанию \"{toDelete.Name}\"?\n" +
                "Вместе с ней будут удалены все её игры и филиалы.",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            List<Game> gamesToDelete = toDelete.Games.ToList();
            for (int i = 0; i < gamesToDelete.Count; ++i)
                Controller.RemoveGame(gamesToDelete[i]);

            List<CompanyBranch> branches = toDelete.CompanyBranches.ToList();
            for (int i = 0; i < branches.Count; ++i)
                Controller.RemoveCompanyBranch(branches[i]);

            if (Controller.RemoveCompany(toDelete))
                MessageBox.Show("Компания успешно удалена");
            else
                MessageBox.Show("Возникла ошибка при удалении компании");
        }
    }
}
