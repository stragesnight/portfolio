using System;
using System.Windows.Forms;

namespace ADO_Exam.View
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void ShowFormDialog(Form toShow)
        {
            this.Hide();
            toShow.ShowDialog();
            this.Show();
        }

        private void button1_Click(object sender, EventArgs e) => ShowFormDialog(new ViewInfoForm());
        private void button6_Click(object sender, EventArgs e) => ShowFormDialog(new ViewStatisticsForm());
        private void button2_Click(object sender, EventArgs e)
            => ShowFormDialog(new AddUpdateInfoForm(AddUpdateInfoForm.WorkMode.AddNewEntry));
        private void button3_Click(object sender, EventArgs e)
            => ShowFormDialog(new AddUpdateInfoForm(AddUpdateInfoForm.WorkMode.UpdateExistingEntry));
        private void button4_Click(object sender, EventArgs e) => ShowFormDialog(new DeleteInfoForm());
        private void button5_Click(object sender, EventArgs e) => Application.Exit();
    }
}
