using System;
using System.Windows.Forms;

namespace NP_Exam_Client
{
    public partial class RenameGroupForm : Form
    {
        private string _oldGroupName = String.Empty; 
        public string NewGroupName = String.Empty;

        public RenameGroupForm(string oldName)
        {
            InitializeComponent();
            _oldGroupName = oldName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Введите имя контакта");
                return;
            }

            NewGroupName = textBox1.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void RenameGroupForm_Load(object sender, EventArgs e)
        {
            textBox1.Text = _oldGroupName;
        }
    }
}
