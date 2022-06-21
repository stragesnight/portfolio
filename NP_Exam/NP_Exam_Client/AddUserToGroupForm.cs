using System;
using NP_Exam_Library;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace NP_Exam_Client
{
    public partial class AddUserToGroupForm : Form
    {
        private RequestSender _rs;
        private DisplayGroup _dg;

        public AddUserToGroupForm(RequestSender sender, DisplayGroup dg)
        {
            InitializeComponent();
            _rs = sender;
            _dg = dg;
        }

        private async Task<DisplayUser> FindUserAsync(RequestSender sender, string username)
        {
            GetUserInfoRequest request = new GetUserInfoRequest(sender, username);
            return await NetworkHelper.GetResponseAsync<DisplayUser>(request);
        }

        private async Task<bool> TryAddUserToGroupAsync(string username)
        {
            DisplayUser user;
            if ((user = await FindUserAsync(_rs, username)) == null)
            {
                MessageBox.Show("Не удалось найти пользователя");
                return false;
            }

            AddUserToGroupRequest request = new AddUserToGroupRequest(_rs, user.Id, _dg.Id);
            if (NetworkHelper.GetResponseAsync<DisplayUserGroup>(request) == null)
            {
                MessageBox.Show("Не удалось добавить пользователя в группу");
                return false;
            }

            MessageBox.Show("Пользователь успешно добавлен");
            return true;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Введите имя пользователя");
                return;
            }

            if (await TryAddUserToGroupAsync(textBox1.Text))
                this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
