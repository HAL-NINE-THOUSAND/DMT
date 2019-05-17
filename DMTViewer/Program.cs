using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using DMT;
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


            //debug localbuild commands
            //    args = new[] {$@"/GameFolder", @"""C:\Games\steamapps\common\7 Days To Die DMT\""", "/InitialPatch" };
            //@"/updatesource \""C:\!Projects\DMT\DMTViewer\bin\Debug/Update/\""/updatedestination "C:\!Projects\DMT\DMTViewer\bin\Debug""

            //if (args != null && args.Length >0)
            //    MessageBox.Show(String.Join("\r\n", args));

           BuildSettings.Load();

            if ( args.Length > 0)
            {
                var data = PatchData.Create(BuildSettings.Instance);
                data.ParseArguments(args);


                if (data.IsUpdate)
                {
                    Updater.Update(data.UpdateSource, data.UpdateDestination);
                    return 0;
                }

                BuildSettings.Instance.Init();
                if (BuildSettings.AutoBuild)
                {
                    new RemoteBuilder().RemoteBuild(new frmMain());
                    return 0;
                }

                return data.Patch();
                
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
    }
}
