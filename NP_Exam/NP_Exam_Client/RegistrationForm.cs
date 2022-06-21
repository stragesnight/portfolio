using System;
using System.Windows.Forms;
using NP_Exam_Library;

namespace NP_Exam_Client
{
    public partial class RegistrationForm : Form
    {
        public RegistrationForm()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text;
            string password = textBox2.Text;

            if (String.IsNullOrEmpty(login) || String.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите пароль и логин");
                return;
            }

            if (textBox2.Text != textBox3.Text)
            {
                MessageBox.Show("Пароли должны совпадать");
                return;
            }

            RegisterUserRequest request = new RegisterUserRequest(textBox1.Text, textBox2.Text);
            if (await NetworkHelper.GetResponseAsync<DisplayUser>(request) != null)
            {
                MessageBox.Show("Вы успешно зарегистрировались");
                this.Close();
            }
            else
            {
                MessageBox.Show("Возникла ошибка при попытке вас зарегистрировать");
            }
        }
    }
}
