using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace SP_Exam
{
    public class Censor
    {
        public enum Status
        {
            Inactive,
            Active,
            Suspended
        };

        public List<string> ForbiddenWords { get; private set; }  = null;
        public Status CensorStatus { get; private set; } = Status.Inactive;

        private List<CensorAnalysisResult> _analysisResults = null;
        private Action<int> _iterationCallback = null; 
        private ManualResetEvent _mre = null;
        private Thread _analysisThread = null;
        private int _entriesAnalyzed = 0;
        private string _resultDir = null;

        public Censor()
        {
            ForbiddenWords = new List<string>();
        }

        public void Start(string dirPath, string resDir, Action<int> iterationCallback, Action<List<CensorAnalysisResult>> resultCallback)
        {
            _analysisResults = new List<CensorAnalysisResult>();
            _iterationCallback = iterationCallback;
            _mre = new ManualResetEvent(true);
            _entriesAnalyzed = 0;
            _resultDir = resDir;

            _analysisThread = new Thread(() => {
                try
                {
                    CensorStatus = Status.Active;
                    FileSystemUtility.ForEachFileParallel(dirPath, AnalyzeEntry, _mre);
                }
                catch (ThreadAbortException)
                { }
                finally
                {
                    CensorStatus = Status.Inactive;
                    resultCallback.Invoke(_analysisResults);
                }
            });

            _analysisThread.Start();
        }

        public void Suspend()
        {
            if (CensorStatus != Status.Active)
                return;

            _mre.Reset();
            CensorStatus = Status.Suspended;
        }

        public void Resume()
        {
            if (CensorStatus != Status.Suspended)
                return;

            _mre.Set();
            CensorStatus = Status.Active;
        }

        public void Stop()
        {
            if (CensorStatus == Status.Inactive)
                return;

            _analysisThread.Abort();
            CensorStatus = Status.Inactive;
        }

        public List<KeyValuePair<string, int>> GetTop10ForbiddenWords(List<CensorAnalysisResult> results)
        {
            Dictionary<string, int> aggregate = new Dictionary<string, int>();

            foreach (CensorAnalysisResult res in results)
            {
                foreach (var item in res.ForbiddenWordsFound)
                {
                    if (aggregate.ContainsKey(item.Key))
                        aggregate[item.Key] += item.Value;
                    else
                        aggregate.Add(item.Key, item.Value);
                }    
            }

            return aggregate.OrderByDescending(x => x.Value).Take(10).ToList();
        }

        private void AnalyzeEntry(string title, string text)
        {
            CensorAnalysisResult res = new CensorAnalysisResult {
                EntryName = title,
                SourceTextLength = text.Length
            };

            foreach (string word in ForbiddenWords)
            {
                int count = CountOf(text, word);
                if (count > 0)
                    res.ForbiddenWordsFound.Add(word, count);
            }

            if (res.ForbiddenWordsFound.Count > 0)
                CreateResultFiles(title, text);

            lock (_analysisResults)
            {
                if (res.ForbiddenWordsFound.Count > 0)
                    _analysisResults.Add(res);
                ++_entriesAnalyzed;
            }

            _iterationCallback?.Invoke(_entriesAnalyzed);
        }

        private int CountOf(string text, string word)
        {
            int res = 0;
            int i = 0;
            int len = text.Length;

            while (i < len - 1 && (i = text.IndexOf(word, i + 1)) >= 0)
                ++res;

            return res;
        }

        private void CreateResultFiles(string srcPath, string text)
        {
            string filename = srcPath.Substring(srcPath.LastIndexOf('\\'));
            string dstPath = _resultDir + filename;
            string redactedDstPath = dstPath.Insert(dstPath.LastIndexOf('.'), "_redacted");

            string redactedText = text;
            foreach (string word in ForbiddenWords)
                redactedText = redactedText.Replace(word, "*******");

            FileSystemUtility.WriteFile(dstPath, text);
            FileSystemUtility.WriteFile(redactedDstPath, redactedText);
        }
    }

    public class CensorAnalysisResult
    {
        public string EntryName { get; set; }
        public int SourceTextLength { get; set; }
        public Dictionary<string, int> ForbiddenWordsFound { get; private set; } = null;

        public CensorAnalysisResult()
        {
            ForbiddenWordsFound = new Dictionary<string, int>();
        }

        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            res.Append($"\"{EntryName}\":\n" +
                $"Длина текста: {SourceTextLength}" +
                $"Всего запрещённых слов: {ForbiddenWordsFound.Values.Sum()}\n" +
                $"Список найденных запрещённых слов:\n");

            foreach (var pair in ForbiddenWordsFound)
                res.AppendLine($" - \"{pair.Key}\": {pair.Value}");

            return res.ToString();
        }
    }
}
