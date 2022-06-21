using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using NP_Exam_Library;

namespace NP_Exam_Client
{
    public partial class CreateDialogForm : Form
    {
        private RequestSender _rs;

        public CreateDialogForm(RequestSender sender)
        {
            InitializeComponent();
            _rs = sender;
        }

        private async Task<bool> TryCreateDialogAsync()
        {
            List<int> userIds = new List<int>();

            try
            {
                userIds.Add((await FindUserAsync(_rs, textBox1.Text)).Id);
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось найти пользователя");
                return false;
            }

            CreateGroupRequest request = new CreateGroupRequest(_rs, $"{_rs.Username}_{textBox1.Text}", userIds);
            DisplayGroup group = await NetworkHelper.GetResponseAsync<DisplayGroup>(request);
            return group != null;
        }

        private async Task<DisplayUser> FindUserAsync(RequestSender sender, string username)
        {
            GetUserInfoRequest request = new GetUserInfoRequest(sender, username);
            return await NetworkHelper.GetResponseAsync<DisplayUser>(request);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Введите имя собеседника");
                return;
            }

            if (await TryCreateDialogAsync())
                this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
