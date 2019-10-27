using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using DMT;

namespace DMTViewer
{
    static class Program
    {

        public static int ExitCode = 0;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //if (args == null || args.Length == 0)
            //args = new[]
            //{
            //    "/Autobuild", "/GameFolder",
            //    @"C:\Games\steamapps\common\7 Days To Die DMT",
            //    "/ModFolder", @"C:\7DaysToDie\DMT Mods",
            //};

            //args = "/LinkedPatch /GameFolder \"../../\" /ScriptOnly".Split(' ');
            //"/Silent /ScriptOnly /GameFolder ../../ /ModFolder ../../Mods"
            if (File.Exists(Application.StartupPath + "/break.txt"))
            {
                Logging.Log("Breaking...");
                return -10;
            }
            //debug localbuild commands
            //    args = new[] {$@"/GameFolder", @"""C:\Games\steamapps\common\7 Days To Die DMT\""", "/InitialPatch" };
            //@"/updatesource \""C:\!Projects\DMT\DMTViewer\bin\Debug/Update/\""/updatedestination "C:\!Projects\DMT\DMTViewer\bin\Debug""

            //if (args != null && args.Length >0)
            //    MessageBox.Show(String.Join("\r\n", args));

            try
            {

                BuildSettings.Load();

                if (args.Length > 0)
                {

                    var data = PatchData.Create(BuildSettings.Instance);


                    data.ParseArguments(args);

                    if (data.IsUpdate)
                    {
                        Logging.Log("Updating...");
                        Updater.Update(data.UpdateSource, data.UpdateDestination);
                        return 0;
                    }

                    BuildSettings.Instance.Init();


                    if (BuildSettings.AutoBuild)
                    {

                        Logging.Log("Auto Build");

                        var t = new System.Threading.Thread(() =>
                        {
                            new RemoteBuilder().RemoteBuild(null, data);
                        });
                        t.Start();

                        while (BuildSettings.AutoBuildComplete == false)
                        {
                            System.Threading.Thread.Sleep(1000);
                            //Logging.Log("Thread complete: " + BuildSettings.AutoBuildComplete);
                        }

                        return 0;
                    }

                    try
                    {

                        var ret = data.Patch();
                        return ret;
                    }
                    catch (Exception e)
                    {
                        Logging.LogError("Nope: " + e.Message);
                        return -11234;
                    }

                }
                else
                {

                    var plugins = Helper.StartFolder() + "/Plugins/";
                    if (Directory.Exists(plugins))
                        foreach (var dll in Directory.GetFiles(plugins, "*.dll"))
                        {
                            AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(dll));
                        }

                    Application.Run(new frmMain());
                    return 0;
                }


            }
            catch (Exception e)
            {

                Logging.Log("Build error: " + e.Message);
                return -99;
            }


        }
    }
}
