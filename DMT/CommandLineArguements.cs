namespace DMT
{
  
    public class CommandGameFolder : ICommandLineArgument
    {
        public bool Apply(string arg, string next, PatchData data)
        {
            if (arg.EqualsIgnoreCase("/GameFolder"))
            {
                var val = next.Replace("\"", "").FolderFormat();
                data.GameFolder = val;
                return true;
            }
            return false;
        }
    }
    public class CommandModsFolder : ICommandLineArgument
    {
        public bool Apply(string arg, string next, PatchData data)
        {
            if (arg.EqualsIgnoreCase("/ModFolder"))
            {
                data.ModFolder = next;
                BuildSettings.Instance.ModFolder = next;
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
