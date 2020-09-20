using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using DMT;
using DMT.Compiler;

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

            if (File.Exists(Application.StartupPath + "/break.txt"))
            {
                Logging.Log("Breaking...");
                return -10;
            }

            try
            {
                Logging.ResetLog();
                BuildSettings.Load();
                BuildSettings.Instance.Compiler = new RoslynCompiler();
                if (args.Length > 0)
                {


                    var data = PatchData.Create(BuildSettings.Instance);
                    data.ParseArguments(args);
                    data.Init();
                    data.Compiler = BuildSettings.Instance.Compiler = BuildSettings.Instance.Compiler  ??  new RoslynCompiler();
                    //Logging.LogError("compiler set to " + BuildSettings.Instance.Compiler.GetType().Name);

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
