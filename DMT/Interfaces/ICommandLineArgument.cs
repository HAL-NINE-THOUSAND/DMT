using DMT;

interface ICommandLineArgument
{
    bool Apply(string arg, string next, PatchData data);
}
