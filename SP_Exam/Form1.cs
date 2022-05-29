using System;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace SP_Exam
{
    public partial class Form1 : Form
    {
        private Censor _censor = null;
        private Semaphore _semaphore = null;
        private int _totalFileCount = 0;
        private int _scannedFileCount = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void EnsureSingleRunningCopy()
        {
            // с семафорами таки работает
            if (Semaphore.TryOpenExisting("SP_EXAM_SEMAPHORE", out _semaphore))
            {
                MessageBox.Show("Другая копия програмы уже запущена!");
                Application.Exit();
            }

            _semaphore = new Semaphore(1, 1, "SP_EXAM_SEMAPHORE");
        }

        private void AddNewForbiddenWord(string word)
        {
            if (String.IsNullOrEmpty(word) || _censor.ForbiddenWords.Contains(word))
                return;

            _censor.ForbiddenWords.Add(word);
            listBox1.Items.Add(word);
        }

        private void RemoveForbiddenWord(string word)
        {
            if (!_censor.ForbiddenWords.Contains(word))
                return;

            _censor.ForbiddenWords.Remove(word);
            listBox1.Items.Remove(word);
        }

        private void UpdateStatusLabel(string text)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(UpdateStatusLabel), text);
                return;
            }

            label4.Text = text;
        }

        private void StartAnalysis()
        {
            if (_censor.ForbiddenWords.Count < 1)
            {
                MessageBox.Show("Добавьте хотя бы одно запрещённое слово");
                return;
            }

            string dir = textBox2.Text;
            string resDir = textBox3.Text;

            if (String.IsNullOrEmpty(dir) || String.IsNullOrEmpty(resDir))
            {
                MessageBox.Show("Введите пути к папкам");
                return;
            }

            new Task(() => {
                UpdateStatusLabel("Вычисление количества файлов...");
                _totalFileCount = FileSystemUtility.GetFileCount(dir);
                UpdateStatusLabel("Запуск поиска...");
                _censor.Start(dir, resDir, IterationCallback, ResultCallback);
            }).Start();

            button5.Text = "Остановить";
            button6.Enabled = true;
        }

        private void StopAnalysis()
        {
            _censor.Stop();
            button5.Text = "Запустить";
            button6.Enabled = false;
            UpdateStatusLabel("Завершено");
        }

        private void IterationCallback(int count)
        {
            this.Invoke(new Action(() => {
                float percent = count / (float)_totalFileCount * 100;
                progressBar1.Value = (int)percent;
                if (_censor.CensorStatus == Censor.Status.Active)
                    UpdateStatusLabel($"сканировано {count} из {_totalFileCount} файлов");

                lock (this)
                    _scannedFileCount = count;
            }));

            Thread.Sleep(50);
        }

        private void ResultCallback(List<CensorAnalysisResult> results)
        {
            string report = GetReport(results);
            string reportName = $"report_{DateTime.Now.ToShortTimeString().Replace(':', '_')}.txt";

            FileSystemUtility.WriteFile(reportName, report);
            Process.Start("notepad.exe", reportName);

            this.Invoke(new Action(StopAnalysis));
        }

        private string GetReport(List<CensorAnalysisResult> results)
        {
            StringBuilder report = new StringBuilder();
            report.AppendLine("СП Экзамен, Шелест");
            report.AppendLine($"Сканирование завершено {DateTime.Now}");
            report.AppendLine($"Количество сканированных файлов: {_scannedFileCount}");
            report.AppendLine();

            report.AppendLine("Топ-10 запрещённых слов:");
            List<KeyValuePair<string, int>> top10 = _censor.GetTop10ForbiddenWords(results);
            for (int i = 0; i < top10.Count; ++i)
                report.AppendLine($"    {i+1}. \"{top10[i].Key}\": {top10[i].Value} шт.");

            report.AppendLine();
            report.AppendLine();

            foreach (var res in results)
            {
                report.AppendLine($"\"{res.EntryName}\": {res.SourceTextLength} байт");

                foreach (var pair in res.ForbiddenWordsFound)
                    report.AppendLine($"    - \"{pair.Key}\": {pair.Value} шт.");

                report.AppendLine();
            }

            return report.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _censor = new Censor();
            EnsureSingleRunningCopy();

            //AddNewForbiddenWord("if");
            //AddNewForbiddenWord("void");
            //AddNewForbiddenWord("new");
            //
            //textBox2.Text = @"C:\Users\win\Desktop\repos_tmp";
            //textBox3.Text = @"C:\Users\win\Desktop\tmp";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddNewForbiddenWord(textBox1.Text);
            textBox1.Text = String.Empty;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            if (FileSystemUtility.ReadFile(ofd.FileName, out string text))
            {
                string[] tokens = text.Split(new char[] { ' ', '\n', '\r' },
                    StringSplitOptions.RemoveEmptyEntries);

                foreach (string item in tokens)
                    AddNewForbiddenWord(item);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
                return;

            RemoveForbiddenWord(listBox1.SelectedItem.ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
                textBox2.Text = fbd.SelectedPath;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
                textBox3.Text = fbd.SelectedPath;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (_censor.CensorStatus == Censor.Status.Inactive)
                StartAnalysis();
            else
                StopAnalysis();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (_censor.CensorStatus == Censor.Status.Active)
            {
                _censor.Suspend();
                button6.Text = "Возобновить";
                UpdateStatusLabel("Приостановлено");
            }
            else
            {
                _censor.Resume();
                button6.Text = "Приостановить";
                UpdateStatusLabel("Возобновление...");
            }
        }
    }
}
