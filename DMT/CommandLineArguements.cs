using DMT.Compiler;

namespace DMT
{
  public class CommandGameFolder : ICommandLineArgument
    {
        public bool Apply(string arg, string next, PatchData data)
        {
            if (arg.EqualsIgnoreCase("/GameFolder") || arg.EqualsIgnoreCase("/GamePath"))
            {
                var val = next.Replace("\"", "").FolderFormat();
                data.GameFolder = val;
                BuildSettings.Instance.GameFolders.Clear();
                BuildSettings.Instance.GameFolders.Add(val);
                return true;
            }
            return false;
        }
    }
  public class CommandModsFolder : ICommandLineArgument
  {
      public bool Apply(string arg, string next, PatchData data)
      {
          if (arg.EqualsIgnoreCase("/ModFolder") || arg.EqualsIgnoreCase("/location"))
          {
              data.ModFolder = next;
              BuildSettings.Instance.ModFolder = next;
              return true;
          }
          return false;
      }
  }


  public class UpdateSourceCommand : ICommandLineArgument
  {
      public bool Apply(string arg, string next, PatchData data)
      {
          if (arg.EqualsIgnoreCase("/UpdateSource"))
          {
              data.UpdateSource = next.Replace("\"", ""); 
              return true;
          }
          return false;
      }
    }
    public class UpdateDestinationCommand : ICommandLineArgument
    {
        public bool Apply(string arg, string next, PatchData data)
        {
            if (arg.EqualsIgnoreCase("/UpdateDestination"))
            {
                data.UpdateDestination = next.Replace("\"", "");
                return true;
            }
            return false;
        }
    }
    public class UseLegacyCompilerCommand : ICommandLineArgument
    {
        public bool Apply(string arg, string next, PatchData data)
        {
            if (arg.EqualsIgnoreCase("/UseLegacyCompiler"))
            {
                data.Compiler = BuildSettings.Instance.Compiler = new CodeDomCompiler();
                return true;
            }
            return false;
        }
    }
    public class BuildIdCommand : ICommandLineArgument
    {
        public bool Apply(string arg, string next, PatchData data)
        {
            if (arg.EqualsIgnoreCase("/BuildId"))
            {
                PatchData.BuildId = next;
                return true;
            }
            return false;
        }
    }
    public class CommandAutoBuild : ICommandLineArgument
    {
        public bool Apply(string arg, string next, PatchData data)
        {
            if (arg.EqualsIgnoreCase("/AutoBuild"))
            {
                BuildSettings.AutoBuild = true;
                return true;
            }
            return false;
        }
    }
    
    public class CommandDisableLocalisation : ICommandLineArgument
    {
        public bool Apply(string arg, string next, PatchData data)
        {
            if (arg.EqualsIgnoreCase("/nolocalisation") || arg.EqualsIgnoreCase("/nolocalization"))
            {
                BuildSettings.DisableLocalisation = true;
                return true;
            }
            return false;
        }
    }
    public class CommandScriptOnly : ICommandLineArgument
    {
        public bool Apply(string arg, string next, PatchData data)
        {
            if (arg.EqualsIgnoreCase("/ScriptOnly"))
            {
                BuildSettings.ScriptOnly = true;
                return true;
            }
            return false;
        }
    }
    public class HarmonyUpdateOnly : ICommandLineArgument
    {
        public bool Apply(string arg, string next, PatchData data)
        {
            if (arg.EqualsIgnoreCase("/UpdateHarmony"))
            {
                BuildSettings.AutoUpdateHarmony = true;
                return true;
            }
            return false;
        }
    }
    public class EnableAllMods : ICommandLineArgument
    {
        public bool Apply(string arg, string next, PatchData data)
        {
            if (arg.EqualsIgnoreCase("/EnableAllMods"))
            {
                BuildSettings.EnableAllMods = true;
                foreach (var m in BuildSettings.Instance.Mods)
                    m.Enabled = true;
                return true;
            }
            return false;
        }
    }
    public class CommandIsSilent : ICommandLineArgument
    {
        public bool Apply(string arg, string next, PatchData data)
        {
            if (arg.EqualsIgnoreCase("/Silent"))
            {
                BuildSettings.IsSilent = true;
                BuildSettings.AutoBuild = true;
                return true;
            }
            return false;
        }
    }
    class CommandSection : ICommandLineArgument
    {
        public bool Apply(string arg, string next, PatchData data)
        {
            if (arg.EqualsIgnoreCase("/InitialPatch"))
            {
                data.RunSection = RunSection.InitialPatch;
                return true;
            }
            if (arg.EqualsIgnoreCase("/LinkedPatch"))
            {
                data.RunSection = RunSection.LinkedPatch;
                return true;
            }
            if (arg.EqualsIgnoreCase("/FinalPatch"))
            {
                data.RunSection = RunSection.FinalPatch;
                return true;
            }
            return false;
        }
    }

}
