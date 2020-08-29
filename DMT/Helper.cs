using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Mono.Cecil;

namespace DMT
{
    public class Helper
    {
        public static void RemoveFolder(string path)
        {
            if (!Directory.Exists(path)) return;
            Directory.Delete(path, true);
        }

        public static string GetRandomString(int length)
        {
            Random random = new Random(DateTime.Now.GetHashCode());
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        public static void ClearTempFolder()
        {
            var tempPath = Path.GetTempPath() + "7DTD-DMT/";

            if (!Directory.Exists(tempPath))
            {
                return;
            }

            var dirs = new DirectoryInfo(tempPath).GetDirectories();

            Logging.LogInfo("Clearing temp folder");

            foreach (var d in dirs)
            {
                try
                {

                    d.Delete(true);
                }
                catch (Exception e)
                {

                }
            }

        }


        public static string StartFolder()
        {
            var ret = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName.FolderFormat();
            return ret;

        }

        public static void MakeFolder(string path)
        {

            if (path == null) return;
            path.MakeFolder();

        }

        public static bool KillProcessByName(string sName)
        {


            try
            {
                Process[] pros = Process.GetProcessesByName(sName);

                Process p = null;


                for (int x = 0; x <= pros.Length - 1; x++)
                {
                    p = pros[x];

                    p.Kill();

                    if (!p.WaitForExit(10000))
                    {
                        return false;
                    }

                }

                return true;
            }
            catch (Exception ex)
            {


            }
            return false;

        }
        public static void CopyFileToDir(string source, string dest)
        {
            var filename = Path.GetFileName(source);
            File.Copy(source, dest.FolderFormat() + filename, true);
        }

        public static void CopyFolder(string from, string to, bool recursive, string ignoreFilename = "")
        {

            to = to.FolderFormat();
            to.MakeFolder();

            foreach (var f in Directory.GetFiles(from))
            {
                var name = Path.GetFileName(f);

                if (name.EqualsIgnoreCase(ignoreFilename))
                    continue;
                File.Copy(f, to + name, true);
            }

            if (recursive)
                foreach (var dir in Directory.GetDirectories(from))
                {
                    var name = new DirectoryInfo(dir).Name;
                    CopyFolder(dir, to + name, recursive);
                }

        }

    }
}
