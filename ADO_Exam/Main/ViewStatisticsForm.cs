using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using ADO_Exam.Repository;

namespace ADO_Exam.View
{
    public partial class ViewStatisticsForm : Form
    {
        public ViewStatisticsForm()
        {
            InitializeComponent();
        }

        private void UpdateVisibilities(bool hasParameter)
        {
            if (button1.InvokeRequired)
            {
                button1.Invoke(new Action<bool>(UpdateVisibilities), hasParameter);
                return;
            }
            if (textBox1.InvokeRequired)
            {
                textBox1.Invoke(new Action<bool>(UpdateVisibilities), hasParameter);
                return;
            }

            button1.Visible = hasParameter;
            textBox1.Visible = hasParameter;
        }

        private void UpdateDataGridView(IEnumerable<object> dataSource)
        {
            if (dataGridView1.InvokeRequired)
            {
                dataGridView1.Invoke(new Action<IEnumerable<object>>(UpdateDataGridView), dataSource);
                return;
            }

            dataGridView1.DataSource = dataSource.ToList();
            ResizeDataGridViewCollumns();
        }

        private void UpdateDataGridView(string hintText)
        {
            if (dataGridView1.InvokeRequired)
            {
                dataGridView1.Invoke(new Action<string>(UpdateDataGridView), hintText);
                return;
            }

            dataGridView1.DataSource = new List<object> { new { Hint = hintText } };
            ResizeDataGridViewCollumns();
        }

        private void ResizeDataGridViewCollumns()
        {
            if (dataGridView1.InvokeRequired)
            {
                dataGridView1.Invoke(new Action(ResizeDataGridViewCollumns));
                return;
            }

            for (int i = 0; i < dataGridView1.Columns.Count; ++i)
                dataGridView1.Columns[i].Width = dataGridView1.Columns[i]
                    .GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, true);
        }

        private void ViewStatisticsForm_Load(object sender, EventArgs e)
        {
            comboBox1.FormattingEnabled = false;
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(StatisticTasks.GetTasks().ToArray());
            comboBox1.SelectedIndex = 0;
            //UpdateVisibilities(false);
        }

        private async void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GameDbTask task = comboBox1.SelectedItem as GameDbTask;
            UpdateVisibilities(task.HasParameter);

            if (!task.HasParameter)
            {
                UpdateDataGridView("Выполнение запроса...");
                UpdateDataGridView(await task.RunAsync());
            }
            else
                UpdateDataGridView("Введите значение параметра");
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            GameDbTask task = comboBox1.SelectedItem as GameDbTask;
            UpdateDataGridView("Выполнение запроса...");
            UpdateDataGridView(await task.RunAsync(textBox1.Text));
        }
    }
}
