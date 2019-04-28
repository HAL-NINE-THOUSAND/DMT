using Mono.Cecil;
using SDX.Compiler;

namespace DMT
{
    public interface IPatch : IPatcherMod
    {

        bool FinalPatch(ModuleDefinition gameModule, ModuleDefinition modModule);

    }
}
namespace SDX.Compiler
{
    public interface IPatcherMod
    {
        bool Patch(ModuleDefinition module);
        bool Link(ModuleDefinition gameModule, ModuleDefinition modModule);
    }
}
