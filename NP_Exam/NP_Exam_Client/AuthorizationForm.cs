using System;
using System.Windows.Forms;
using NP_Exam_Library;

namespace NP_Exam_Client
{
    public partial class AuthorizationForm : Form
    {
        public AuthorizationForm()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox1.Text) || String.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Введите имя пользователя и пароль");
                return;
            }

            RequestSender rs = new RequestSender(-1, textBox1.Text, textBox2.Text);
            AuthorizeUserRequest request = new AuthorizeUserRequest(rs);
            DisplayUser user;

            if ((user = await NetworkHelper.GetResponseAsync<DisplayUser>(request)) != null)
            {
                rs.Id = user.Id;
                this.Hide();
                new MainForm(rs).ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Не удалось авторизироваться");
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            new RegistrationForm().ShowDialog();
            this.Show();
        }
    }
}
