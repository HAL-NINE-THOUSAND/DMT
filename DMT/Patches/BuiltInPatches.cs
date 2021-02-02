using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace DMT.Patches
{
    class BuiltInPatches
    {

        internal void Patch(ModuleDefinition game, ModuleDefinition dmt)
        {
            MarkAsPatched(game);
            HookHarmony(game, dmt);
            HookConsoleCommands(game, dmt);
            UpdateFindTypeHelper(game, dmt);
        }

        private void MarkAsPatched(ModuleDefinition game)
        {
            var fieldDef = new FieldDefinition("SdxPatchedCheck", FieldAttributes.Public | FieldAttributes.Static, game.Import(typeof(bool)));
            var modManager = game.Types.FirstOrDefault(d => d.Name == "ModManager");
            modManager.Fields.Add(fieldDef);
        }

        private void HookHarmony(ModuleDefinition game, ModuleDefinition mod)
        {
            Logging.Log("Hooking harmony");

            var steam = game.Types.FirstOrDefault(d => d.Name == "Steam");
            var singletonCreated = steam.Methods.First(d => d.Name == "singletonCreated");
            var helper = mod.Types.First(d => d.Name == "DMTChanges");
            var hookHarmony = game.Import(helper.Methods.First(d => d.Name == "HookHarmony"));

            var pro = singletonCreated.Body.GetILProcessor();
            var body = pro.Body.Instructions;
            var ins = body.First(d => d.OpCode == OpCodes.Newobj);

            pro.InsertBefore(ins, Instruction.Create(OpCodes.Callvirt, hookHarmony));


        }
        private void UpdateFindTypeHelper(ModuleDefinition game, ModuleDefinition mod)
        {
          
            var console = game.Types.FirstOrDefault(d => d.Name == "ReflectionHelpers");
            var register = console.Methods.First(d => d.Name == "FindTypesImplementingBase");

            var helper = mod.Types.First(d => d.Name == "DMTChanges");
            var getAsses = game.ImportReference(helper.Methods.First(d => d.Name == "GetLoadedAssemblies"));

            var pro = register.Body.GetILProcessor();
            var body = pro.Body.Instructions;
            var ins = body.First(d=> d.Operand != null && d.Operand.ToString().ContainsIgnoreCase("GetExecutingAssembly"));

            if (ins == null)
            {
                Logging.LogWarning("Could not update find type helper");
                return;
            }
            pro.Body.Instructions[0].Operand = getAsses;
            pro.Remove(ins.Next);
            pro.Remove(ins.Previous.Previous);
            pro.Remove(ins.Previous);
            pro.Remove(ins);

        }
        private void HookConsoleCommands(ModuleDefinition game, ModuleDefinition mod)
        {
            Logging.Log("Hooking console commands");

            var console = game.Types.FirstOrDefault(d => d.Name == "SdtdConsole");
            var register = console.Methods.First(d => d.Name == "RegisterCommands");
            var helper = mod.Types.First(d => d.Name == "DMTChanges");
            var addRefs = game.Import(helper.Methods.First(d => d.Name == "AddReferencedAssemblies"));
            var initMethods = game.Import(helper.Methods.First(d => d.Name == "DmtInit"));

            var pro = register.Body.GetILProcessor();
            var body = pro.Body.Instructions;
            var ins = body.First();

            //pro.InsertBefore(ins, Instruction.Create(OpCodes.Ldloc_1));
            pro.InsertBefore(ins, Instruction.Create(OpCodes.Callvirt, addRefs));
            pro.InsertBefore(ins, Instruction.Create(OpCodes.Callvirt, initMethods));


        }
    }
}