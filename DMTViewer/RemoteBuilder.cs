using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DMTViewer;

namespace DMT
{
    internal class RemoteBuilder
    {
        private frmMain Form;

        private List<string> BuildArguments = new List<string>();
        Process SdxProcess = new Process();

        private DateTime Start;

        internal void InternalBuild(frmMain frm)
        {

            Logging.LogAction = ParseLog;
            Start = DateTime.Now;
            Form = frm;

            Form.DisableButtons();

            var data = PatchData.Create(BuildSettings.Instance);
            data.GameFolder = BuildSettings.Instance.GameFolders[0];
            data.Init();
            data.RunSection = RunSection.InitialPatch;
            var ret = data.Patch();

            data.RunSection = RunSection.LinkedPatch;
            BuildSettings.Instance.Init();
            ret = data.Patch();

            data.RunSection = RunSection.FinalPatch;
            BuildSettings.Instance.Init();
            ret = data.Patch();
            

        }

        internal void RemoteBuild(frmMain frm)
        {

            Start = DateTime.Now;
            Form = frm;
            Form.DisableButtons();

            var data = PatchData.Create(BuildSettings.Instance);

            foreach (var location in BuildSettings.Instance.GameFolders)
            {
                data.GameFolder = location;

                if (BuildSettings.Instance.AutoClose)
                {
                    var isDedicatedServer = data.IsDedicatedServer;
                    var exe = isDedicatedServer ? "7DaysToDieServer" : "7DaysToDie";
                    Helper.KillProcessByName(exe);
                }

                BuildArguments.Add($@"/InitialPatch /GameFolder ""{data.GameFolder}""");
                BuildArguments.Add($@"/LinkedPatch /GameFolder ""{data.GameFolder}""");
                BuildArguments.Add($@"/FinalPatch /GameFolder ""{data.GameFolder}""");

                if (BuildSettings.Instance.AutoPlay)
                    BuildArguments.Add($@"startprocess {data.StartPath}");

            }
            Next();
        }

        private void Next()
        {

            if (BuildArguments.Count == 0)
            {
                Form.Invoke(new Action(() => { Form.EnableButtons(); Form.OnLog($"{DateTime.Now.ToString("HH:mm:ss")}: Build completed in {Math.Round((DateTime.Now - Start).TotalSeconds, 2)} seconds", LogType.Event); }));
                if (Form.chkPlay.Checked)
                {
                    Form.Play_Click(null, null);
                }
                return;
            }

            var next = BuildArguments[0];
            BuildArguments.RemoveAt(0);

            //dr - hacky
            if (next.StartsWith("startprocess "))
            {
                //Process.Start(next.Substring(13));
                Next();
                return;
            }

            WaitForProcess(next);

        }

        private bool WaitForProcess(string args)
        {



            SdxProcess= new Process();
            SdxProcess.EnableRaisingEvents = true;
            SdxProcess.OutputDataReceived += process_OutputDataReceived;
            SdxProcess.ErrorDataReceived += process_ErrorDataReceived;
            SdxProcess.Exited += process_Exited;

            SdxProcess.StartInfo.FileName = Assembly.GetExecutingAssembly().Location;
            SdxProcess.StartInfo.Arguments = args; // "/autobuild /silent"; ///location C:\\SQL\\" 
            SdxProcess.StartInfo.UseShellExecute = false;
            SdxProcess.StartInfo.RedirectStandardError = true;
            SdxProcess.StartInfo.RedirectStandardOutput = true;

            SdxProcess.Start();
            SdxProcess.BeginErrorReadLine();
            SdxProcess.BeginOutputReadLine();
           // SdxProcess.WaitForExit();

            return true;
        }

        void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {

            var text = e.Data ?? String.Empty;
            ParseLog(text);

        }

        public void ParseLog(string text)
        {

            int i = 0;
            var typeString = "";
            while (true)
            {
                if (i >= text.Length)
                {
                    typeString = "0";
                    break;
                }

                Char c = text[i++];
                if (c == '|')
                    break;
                typeString += c;
            }

            var msg = text.Substring(i);

            int type;
            int.TryParse(typeString, out type);

            Form.Invoke(new Action(() => { Form.OnLog(msg, (LogType)type); }));
        }

        void process_Exited(object sender, EventArgs e)
        {

            var pro = sender as Process;
            if (pro != null && pro.ExitCode < 0)
            {
                Form.Invoke(new Action(() => { Form.OnLog($"process exited with error code " + pro.ExitCode, LogType.Error); }));
                Form.Invoke(new Action(() => { Form.TryFindError(); Form.EnableButtons(); }));
                return;
            }
            Next();

        }

        void process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {

            Form.Invoke(new Action(() => { Form.OnLog(e.Data, LogType.Error); }));
        }

    }
}
