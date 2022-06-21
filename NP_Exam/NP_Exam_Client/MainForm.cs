using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using NP_Exam_Library;

namespace NP_Exam_Client
{
    public partial class MainForm : Form
    {
        private RequestSender _rs = null;
        private DisplayGroup _selectedGroup = null;
        private bool _shouldExit = false;
        private MessageAttachment _attach = null;
        private int _selectedGroupIndex = 0;
        private int _selectedMsgIndex = 0;
        private int _prevGroupCount = 0;

        public MainForm(RequestSender rs)
        {
            InitializeComponent();
            _rs = rs;
        }

        private void UpdateLoop(object state)
        {
            while (!_shouldExit)
            {
                try
                {
                    LoadGroupListAsync().Wait();
                    if (_selectedGroup != null)
                        LoadGroupMessagesAsync(_selectedGroup).Wait();
                }
                catch (Exception)
                { }

                Thread.Sleep(1000);
            }
        }

        private async Task LoadGroupListAsync()
        {
            GetGroupListRequest request = new GetGroupListRequest(_rs);
            List<DisplayGroup> groups = await NetworkHelper.GetResponseAsync<List<DisplayGroup>>(request);
            if (groups == null)
            {
                //MessageBox.Show("Не удалось загрузить список контактов");
                return;
            }

            this.Invoke((MethodInvoker)(() => {
                listBox1.Items.Clear();
                listBox1.Items.AddRange(groups.ToArray());
                if (listBox1.Items.Count > _selectedGroupIndex)
                    listBox1.SelectedIndex = _selectedGroupIndex;
                _prevGroupCount = groups.Count;
            }));
        }

        private async Task LoadGroupMessagesAsync(DisplayGroup group)
        {
            if (group == null)
                return;

            GetGroupMessagesRequest request = new GetGroupMessagesRequest(_rs, group.Id);
            List<DisplayMessage> messages = await NetworkHelper.GetResponseAsync<List<DisplayMessage>>(request);
            if (messages == null)
            {
                //MessageBox.Show("Не удалось получить список сообщений");
                return;
            }

            this.Invoke((MethodInvoker)(() => {
                listBox2.Items.Clear();
                listBox2.Items.AddRange(messages.ToArray());
                panel1.Visible = true;
                if (listBox2.Items.Count > _selectedMsgIndex)
                    listBox2.SelectedIndex = _selectedMsgIndex;
            }));
        }

        private async Task SendMessageAsync(string msgText, object attach)
        {
            SendMessageRequest request = new SendMessageRequest(_rs, _selectedGroup.Id, msgText, attach);
            if (await NetworkHelper.GetResponseAsync<DisplayMessage>(request) == null)
            {
                //MessageBox.Show("Не удалось отправить сообщение");
                return;
            }

            this.Invoke((MethodInvoker)(() => {
                listBox2.Items.Add(new DisplayMessage(_rs.Id, _selectedGroup.Id, msgText, _rs.Username, attach));
                Task _ = LoadGroupMessagesAsync(_selectedGroup);
                textBox1.Text = String.Empty;
                _attach = null;
            }));
        }

        private async Task DeauthorizeAsync()
        {
            DeauthorizeUserRequest request = new DeauthorizeUserRequest(_rs);
            await NetworkHelper.GetResponseAsync<DisplayUser>(request);
        }

        private async Task LeaveGroupAsync()
        {
            LeaveGroupRequest request = new LeaveGroupRequest(_rs, _selectedGroup.Id);
            await NetworkHelper.GetResponseAsync<DisplayUserGroup>(request);
            this.Invoke((MethodInvoker)(() => {
                panel1.Visible = false;
                _selectedGroup = null;
            }));
        }

        private async Task RenameGroupAsync(string newName)
        {
            RenameGroupRequest request = new RenameGroupRequest(_rs, _selectedGroup.Id, newName);
            await NetworkHelper.GetResponseAsync<DisplayGroup>(request);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (_rs == null)
            {
                MessageBox.Show("Что-то пошло не так");
                this.Close();
            }

            listBox1.FormattingEnabled = false;
            listBox2.FormattingEnabled = false;

            listBox1.ContextMenuStrip = contextMenuStrip2;
            listBox2.ContextMenuStrip = contextMenuStrip1;

            panel1.Visible = false;

            ThreadPool.QueueUserWorkItem(UpdateLoop);

            this.Closing += (s, e1) => {
                _shouldExit = true;
                Task _ = DeauthorizeAsync();
            };
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayGroup group = listBox1.SelectedItem as DisplayGroup;
            _selectedGroup = group;
            _selectedGroupIndex = listBox1.SelectedIndex;
            Task _ = LoadGroupMessagesAsync(group);
        }


        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedMsgIndex = listBox2.SelectedIndex;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string msg = textBox1.Text;
            Task _ = SendMessageAsync(msg, _attach);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new CreateDialogForm(_rs).ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new AddUserToGroupForm(_rs, _selectedGroup).ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Task _ = LeaveGroupAsync();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            using (BinaryReader br = new BinaryReader(File.OpenRead(ofd.FileName)))
            {
                byte[] bytes = br.ReadBytes((int)br.BaseStream.Length);
                string filename = ofd.FileName.Substring(ofd.FileName.LastIndexOf('\\') + 1);
                _attach = new MessageAttachment(filename, bytes);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DisplayMessage msg = listBox2.SelectedItem as DisplayMessage;
            if (msg.Attachment == null)
            {
                MessageBox.Show("К этому сообщению нету прикреплённого файла");
                return;
            }

            string basedir = AppDomain.CurrentDomain.BaseDirectory;
            MessageAttachment a = msg.Attachment as MessageAttachment;
            using (BinaryWriter bw = new BinaryWriter(File.Open(a.Filename, FileMode.CreateNew)))
                bw.Write(a.Data);

            MessageBox.Show($"Файл сохранён в {basedir}{a.Filename}");
        }

        private void listBox2_MouseDown(object sender, MouseEventArgs e)
        {
            listBox2.SelectedIndex = listBox2.IndexFromPoint(e.Location);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            RenameGroupForm form = new RenameGroupForm(_selectedGroup.GroupName);
            if (form.ShowDialog() != DialogResult.OK)
                return;

            Task _ = RenameGroupAsync(form.NewGroupName);
        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            listBox1.SelectedIndex = listBox1.IndexFromPoint(e.Location);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button1_Click(sender, e);
        }
    }
}
