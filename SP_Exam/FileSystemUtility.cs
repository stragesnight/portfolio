using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

namespace SP_Exam
{
    public static class FileSystemUtility
    {
        public static bool ReadFile(string path, out string text)
        {
            try
            {
                using (StreamReader sr = new StreamReader(path))
                    text = sr.ReadToEnd();
                return true;
            }
            catch (Exception)
            {
                text = String.Empty;
                return false;
            }
        }

        public static bool WriteFile(string path, string text)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path))
                    sw.Write(text);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool DeleteFile(string path)
        {
            try
            {
                File.Delete(path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool CopyFile(string srcPath, string dstPath)
        {
            bool res = true;
            res &= ReadFile(srcPath, out string srcText);
            res &= WriteFile(dstPath, srcText);

            return res;
        }

        public static bool ForEachFileParallel(string dirPath, Action<string, string> action, WaitHandle waitHandle = null)
        {
            Stack<string> pendingDirs = new Stack<string>();
            pendingDirs.Push(dirPath);

            while (pendingDirs.Count > 0)
            {
                string dir = pendingDirs.Pop();
                Directory.GetFiles(dir).AsParallel().ForAll(x => {
                    waitHandle?.WaitOne();
                    if (ReadFile(x, out string text))
                        action.Invoke(x, text);
                });

                foreach (string item in Directory.GetDirectories(dir))
                    pendingDirs.Push(item);
            }

            return true;
        }

        public static int GetFileCount(string dirPath)
        {
            int res = 0;
            Stack<string> pendingDirs = new Stack<string>();
            pendingDirs.Push(dirPath);

            while (pendingDirs.Count > 0)
            {
                string dir = pendingDirs.Pop();
                res += Directory.GetFiles(dir).Count();

                foreach (string item in Directory.GetDirectories(dir))
                    pendingDirs.Push(item);
            }

            return res;
        }
    }
}
